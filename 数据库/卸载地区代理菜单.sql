-- 地区代理管理菜单卸载脚本
-- 警告：此脚本将完全移除地区代理管理相关的菜单配置
-- 执行前请确认您确实需要卸载这些功能

-- 开始事务（建议在支持事务的数据库中使用）
-- BEGIN TRANSACTION;

-- 1. 显示即将删除的菜单项（用于确认）
SELECT 
    '即将删除的菜单项' AS 操作,
    id AS 菜单ID,
    menuName AS 菜单名称,
    CASE 
        WHEN menuType = 0 THEN '页面菜单'
        WHEN menuType = 1 THEN 'API权限'
        ELSE '其他类型'
    END AS 菜单类型
FROM SysMenu 
WHERE id BETWEEN 1200 AND 1225
ORDER BY id;

-- 2. 删除角色菜单关联（如果存在SysRoleMenu表）
-- 注意：根据您的系统实际情况，可能需要调整表名
DELETE FROM SysRoleMenu WHERE menuId BETWEEN 1200 AND 1225;

-- 3. 删除地区代理相关的所有菜单项
DELETE FROM SysMenu WHERE id BETWEEN 1200 AND 1225;

-- 4. 验证删除结果
SELECT 
    '删除结果验证' AS 验证项目,
    COUNT(*) AS 剩余数量,
    CASE WHEN COUNT(*) = 0 THEN '✓ 删除成功' ELSE '✗ 删除不完整' END AS 状态
FROM SysMenu 
WHERE id BETWEEN 1200 AND 1225;

-- 5. 显示代理设置菜单下的剩余子菜单
SELECT 
    '代理设置子菜单' AS 信息,
    id AS 菜单ID,
    menuName AS 菜单名称,
    sortNumber AS 排序号
FROM SysMenu 
WHERE parentId = 630
ORDER BY sortNumber;

-- 提交事务（如果使用了事务）
-- COMMIT TRANSACTION;

-- 卸载完成提示
SELECT 
    '卸载完成' AS 状态,
    '地区代理管理菜单已成功移除' AS 消息,
    '请重启应用程序使更改生效' AS 建议;

/*
卸载说明：
1. 此脚本会删除ID范围在1200-1225的所有菜单项
2. 同时会删除相关的角色权限关联
3. 执行后需要重启CoreShop应用程序
4. 如果需要恢复，请重新执行安装脚本

注意事项：
- 执行前请备份数据库
- 确认没有其他功能依赖这些菜单项
- 建议在测试环境中先验证脚本效果
*/