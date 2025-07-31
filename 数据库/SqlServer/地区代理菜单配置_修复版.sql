-- 地区代理管理菜单配置 (修复版 - 避免ID冲突)
-- 使用新的ID范围：1300-1325
-- 添加地区代理相关的菜单项到SysMenu表

-- 检查ID是否已存在
SELECT 
    '冲突检查' AS 检查项目,
    COUNT(*) AS 冲突数量,
    CASE WHEN COUNT(*) > 0 THEN '存在冲突，请手动调整ID' ELSE '无冲突，可以安装' END AS 状态
FROM [SysMenu] 
WHERE id BETWEEN 1300 AND 1325;

-- 如果上述查询结果显示无冲突，则继续执行以下语句

-- 1. 地区代理管理主菜单
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1300, 630, 'agentArea', N'地区代理管理', '', 'agent/agentArea/index', '', 0, 15, 'agent:agentArea:index', null, null, '0', '0', GETDATE(), null);

-- 2. 地区代理订单记录主菜单
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1301, 630, 'agentAreaOrder', N'地区代理订单记录', '', 'agent/agentAreaOrder/index', '', 0, 16, 'agent:agentAreaOrder:index', null, null, '0', '0', GETDATE(), null);

-- 3. 地区代理管理相关API权限菜单
-- 获取列表
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1302, 1300, 'GetPageList', N'获取列表', null, null, '/Api/CoreCmsAgentArea/GetPageList', 1, 0, 'CoreCmsAgentArea:GetPageList', null, null, '0', '0', GETDATE(), null);

-- 首页数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1303, 1300, 'GetIndex', N'首页数据', null, null, '/Api/CoreCmsAgentArea/GetIndex', 1, 1, 'CoreCmsAgentArea:GetIndex', null, null, '0', '0', GETDATE(), null);

-- 创建数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1304, 1300, 'GetCreate', N'创建数据', null, null, '/Api/CoreCmsAgentArea/GetCreate', 1, 2, 'CoreCmsAgentArea:GetCreate', null, null, '0', '0', GETDATE(), null);

-- 创建提交
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1305, 1300, 'DoCreate', N'创建提交', null, null, '/Api/CoreCmsAgentArea/DoCreate', 1, 3, 'CoreCmsAgentArea:DoCreate', null, null, '0', '0', GETDATE(), null);

-- 编辑数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1306, 1300, 'GetEdit', N'编辑数据', null, null, '/Api/CoreCmsAgentArea/GetEdit', 1, 4, 'CoreCmsAgentArea:GetEdit', null, null, '0', '0', GETDATE(), null);

-- 编辑提交
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1307, 1300, 'DoEdit', N'编辑提交', null, null, '/Api/CoreCmsAgentArea/DoEdit', 1, 5, 'CoreCmsAgentArea:DoEdit', null, null, '0', '0', GETDATE(), null);

-- 单选删除
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1308, 1300, 'DoDelete', N'单选删除', null, null, '/Api/CoreCmsAgentArea/DoDelete', 1, 6, 'CoreCmsAgentArea:DoDelete', null, null, '0', '0', GETDATE(), null);

-- 批量删除
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1309, 1300, 'DoBatchDelete', N'批量删除', null, null, '/Api/CoreCmsAgentArea/DoBatchDelete', 1, 7, 'CoreCmsAgentArea:DoBatchDelete', null, null, '0', '0', GETDATE(), null);

-- 预览数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1310, 1300, 'GetDetails', N'预览数据', null, null, '/Api/CoreCmsAgentArea/GetDetails', 1, 8, 'CoreCmsAgentArea:GetDetails', null, null, '0', '0', GETDATE(), null);

-- 设置启用状态
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1311, 1300, 'DoSetIsEnable', N'设置启用状态', null, null, '/Api/CoreCmsAgentArea/DoSetIsEnable', 1, 9, 'CoreCmsAgentArea:DoSetIsEnable', null, null, '0', '0', GETDATE(), null);

-- 获取代理商列表
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1312, 1300, 'GetAgentList', N'获取代理商列表', null, null, '/Api/CoreCmsAgentArea/GetAgentList', 1, 10, 'CoreCmsAgentArea:GetAgentList', null, null, '0', '0', GETDATE(), null);

-- 获取地区树
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1313, 1300, 'GetAreaTree', N'获取地区树', null, null, '/Api/CoreCmsAgentArea/GetAreaTree', 1, 11, 'CoreCmsAgentArea:GetAreaTree', null, null, '0', '0', GETDATE(), null);

-- 获取省市县列表
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1314, 1300, 'GetAreaList', N'获取省市县列表', null, null, '/Api/CoreCmsAgentArea/GetAreaList', 1, 12, 'CoreCmsAgentArea:GetAreaList', null, null, '0', '0', GETDATE(), null);

-- 4. 地区代理订单记录相关API权限菜单
-- 获取列表
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1315, 1301, 'GetPageList', N'获取列表', null, null, '/Api/CoreCmsAgentAreaOrder/GetPageList', 1, 0, 'CoreCmsAgentAreaOrder:GetPageList', null, null, '0', '0', GETDATE(), null);

-- 首页数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1316, 1301, 'GetIndex', N'首页数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetIndex', 1, 1, 'CoreCmsAgentAreaOrder:GetIndex', null, null, '0', '0', GETDATE(), null);

-- 预览数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1317, 1301, 'GetDetails', N'预览数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetDetails', 1, 2, 'CoreCmsAgentAreaOrder:GetDetails', null, null, '0', '0', GETDATE(), null);

-- 编辑提交
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1318, 1301, 'DoEdit', N'编辑提交', null, null, '/Api/CoreCmsAgentAreaOrder/DoEdit', 1, 3, 'CoreCmsAgentAreaOrder:DoEdit', null, null, '0', '0', GETDATE(), null);

-- 获取统计数据
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1319, 1301, 'GetStatistics', N'获取统计数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetStatistics', 1, 4, 'CoreCmsAgentAreaOrder:GetStatistics', null, null, '0', '0', GETDATE(), null);

-- 结算操作
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1320, 1301, 'DoSettlement', N'结算操作', null, null, '/Api/CoreCmsAgentAreaOrder/DoSettlement', 1, 5, 'CoreCmsAgentAreaOrder:DoSettlement', null, null, '0', '0', GETDATE(), null);

-- 批量结算
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1321, 1301, 'DoBatchSettlement', N'批量结算', null, null, '/Api/CoreCmsAgentAreaOrder/DoBatchSettlement', 1, 6, 'CoreCmsAgentAreaOrder:DoBatchSettlement', null, null, '0', '0', GETDATE(), null);

-- 设置结算状态
INSERT INTO [SysMenu] ([id], [parentId], [identificationCode], [menuName], [menuIcon], [path], [component], [menuType], [sortNumber], [authority], [target], [iconColor], [hide], [deleted], [createTime], [updateTime]) 
VALUES (1322, 1301, 'DoSetSettlementStatus', N'设置结算状态', null, null, '/Api/CoreCmsAgentAreaOrder/DoSetSettlementStatus', 1, 7, 'CoreCmsAgentAreaOrder:DoSetSettlementStatus', null, null, '0', '0', GETDATE(), null);

-- 安装完成验证
SELECT 
    '安装完成验证' AS 验证项目,
    COUNT(*) AS 已安装数量,
    CASE WHEN COUNT(*) = 21 THEN '✓ 安装成功' ELSE '✗ 安装不完整' END AS 状态
FROM [SysMenu] 
WHERE id BETWEEN 1300 AND 1322;

-- 显示新安装的主菜单
SELECT 
    id AS 菜单ID,
    menuName AS 菜单名称,
    path AS 路径,
    authority AS 权限标识
FROM [SysMenu] 
WHERE id IN (1300, 1301)
ORDER BY id;