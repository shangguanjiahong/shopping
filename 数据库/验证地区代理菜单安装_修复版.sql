-- 地区代理管理菜单安装验证脚本 (修复版)
-- 使用新的ID范围：1300-1322
-- 执行此脚本来验证菜单是否正确安装

-- 1. 检查主菜单项是否存在
SELECT 
    '主菜单检查' AS 检查项目,
    COUNT(*) AS 已安装数量,
    CASE WHEN COUNT(*) = 2 THEN '✓ 正常' ELSE '✗ 异常' END AS 状态
FROM SysMenu 
WHERE id IN (1300, 1301);

-- 2. 显示主菜单详情
SELECT 
    id AS 菜单ID,
    menuName AS 菜单名称,
    path AS 路径,
    authority AS 权限标识,
    parentId AS 父级ID,
    sortNumber AS 排序号
FROM SysMenu 
WHERE id IN (1300, 1301)
ORDER BY id;

-- 3. 检查API权限菜单数量
SELECT 
    'API权限菜单检查' AS 检查项目,
    COUNT(*) AS 已安装数量,
    CASE WHEN COUNT(*) = 21 THEN '✓ 正常' ELSE '✗ 异常' END AS 状态
FROM SysMenu 
WHERE id BETWEEN 1302 AND 1322;

-- 4. 显示地区代理管理API权限
SELECT 
    id AS 菜单ID,
    menuName AS 菜单名称,
    component AS API路径,
    authority AS 权限标识
FROM SysMenu 
WHERE parentId = 1300
ORDER BY id;

-- 5. 显示地区代理订单API权限
SELECT 
    id AS 菜单ID,
    menuName AS 菜单名称,
    component AS API路径,
    authority AS 权限标识
FROM SysMenu 
WHERE parentId = 1301
ORDER BY id;

-- 6. 检查父级菜单（代理设置）是否存在
SELECT 
    '父级菜单检查' AS 检查项目,
    COUNT(*) AS 数量,
    CASE WHEN COUNT(*) > 0 THEN '✓ 正常' ELSE '✗ 父级菜单不存在' END AS 状态
FROM SysMenu 
WHERE id = 630;

-- 7. 显示代理设置下的所有子菜单
SELECT 
    id AS 菜单ID,
    menuName AS 菜单名称,
    path AS 路径,
    sortNumber AS 排序号,
    CASE WHEN hide = '0' THEN '显示' ELSE '隐藏' END AS 显示状态
FROM SysMenu 
WHERE parentId = 630
ORDER BY sortNumber;

-- 8. 检查是否有ID冲突
SELECT 
    'ID冲突检查' AS 检查项目,
    COUNT(*) AS 重复数量,
    CASE WHEN COUNT(*) = 0 THEN '✓ 无冲突' ELSE '✗ 存在ID冲突' END AS 状态
FROM (
    SELECT id, COUNT(*) as cnt
    FROM SysMenu 
    WHERE id BETWEEN 1300 AND 1322
    GROUP BY id
    HAVING COUNT(*) > 1
) AS duplicates;

-- 9. 检查旧版本ID是否存在（应该不存在）
SELECT 
    '旧版本ID检查' AS 检查项目,
    COUNT(*) AS 旧版本数量,
    CASE WHEN COUNT(*) = 0 THEN '✓ 无旧版本残留' ELSE '✗ 存在旧版本，需要清理' END AS 状态
FROM SysMenu 
WHERE id BETWEEN 1200 AND 1225;

-- 10. 总体安装状态检查
SELECT 
    '总体安装状态' AS 检查项目,
    CASE 
        WHEN (
            SELECT COUNT(*) FROM SysMenu WHERE id IN (1300, 1301)
        ) = 2 
        AND (
            SELECT COUNT(*) FROM SysMenu WHERE id BETWEEN 1302 AND 1322
        ) = 21
        AND (
            SELECT COUNT(*) FROM SysMenu WHERE id = 630
        ) > 0
        THEN '✓ 安装成功'
        ELSE '✗ 安装不完整'
    END AS 状态;

-- 11. 显示完整的菜单结构
SELECT 
    '完整菜单结构' AS 信息,
    CASE 
        WHEN parentId IS NULL THEN CAST(id AS VARCHAR) + '. ' + menuName
        WHEN parentId = 630 THEN '  ├── ' + CAST(id AS VARCHAR) + '. ' + menuName
        WHEN parentId IN (1300, 1301) THEN '    ├── ' + CAST(id AS VARCHAR) + '. ' + menuName
        ELSE '      ├── ' + CAST(id AS VARCHAR) + '. ' + menuName
    END AS 菜单结构
FROM SysMenu 
WHERE id = 630 OR parentId = 630 OR parentId IN (1300, 1301)
ORDER BY 
    CASE WHEN id = 630 THEN 0 ELSE 1 END,
    CASE WHEN parentId = 630 THEN sortNumber ELSE 999 END,
    CASE WHEN parentId IN (1300, 1301) THEN sortNumber ELSE 999 END;

-- 12. 权限分配建议
SELECT 
    '权限分配提醒' AS 提醒事项,
    '请在角色管理中为相应角色分配地区代理管理权限' AS 操作建议,
    '新的菜单ID范围：1300-1322' AS 备注;

-- 13. 如果需要清理旧版本数据
SELECT 
    '清理旧版本提示' AS 提示,
    CASE 
        WHEN (SELECT COUNT(*) FROM SysMenu WHERE id BETWEEN 1200 AND 1225) > 0
        THEN '发现旧版本数据，建议执行：DELETE FROM SysMenu WHERE id BETWEEN 1200 AND 1225;'
        ELSE '无需清理旧版本数据'
    END AS 建议;