-- 地区代理管理菜单配置
-- 添加地区代理相关的菜单项到SysMenu表

-- 1. 地区代理管理主菜单
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1200, 630, 'agentArea', '地区代理管理', '', 'agent/agentArea/index', '', 0, 15, 'agent:agentArea:index', null, null, '0', '0', NOW(), null);

-- 2. 地区代理订单记录主菜单
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1201, 630, 'agentAreaOrder', '地区代理订单记录', '', 'agent/agentAreaOrder/index', '', 0, 16, 'agent:agentAreaOrder:index', null, null, '0', '0', NOW(), null);

-- 3. 地区代理管理相关API权限菜单
-- 获取列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1202, 1200, 'GetPageList', '获取列表', null, null, '/Api/CoreCmsAgentArea/GetPageList', 1, 0, 'CoreCmsAgentArea:GetPageList', null, null, '0', '0', NOW(), null);

-- 首页数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1203, 1200, 'GetIndex', '首页数据', null, null, '/Api/CoreCmsAgentArea/GetIndex', 1, 1, 'CoreCmsAgentArea:GetIndex', null, null, '0', '0', NOW(), null);

-- 创建数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1204, 1200, 'GetCreate', '创建数据', null, null, '/Api/CoreCmsAgentArea/GetCreate', 1, 2, 'CoreCmsAgentArea:GetCreate', null, null, '0', '0', NOW(), null);

-- 创建提交
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1205, 1200, 'DoCreate', '创建提交', null, null, '/Api/CoreCmsAgentArea/DoCreate', 1, 3, 'CoreCmsAgentArea:DoCreate', null, null, '0', '0', NOW(), null);

-- 编辑数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1206, 1200, 'GetEdit', '编辑数据', null, null, '/Api/CoreCmsAgentArea/GetEdit', 1, 4, 'CoreCmsAgentArea:GetEdit', null, null, '0', '0', NOW(), null);

-- 编辑提交
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1207, 1200, 'DoEdit', '编辑提交', null, null, '/Api/CoreCmsAgentArea/DoEdit', 1, 5, 'CoreCmsAgentArea:DoEdit', null, null, '0', '0', NOW(), null);

-- 单选删除
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1208, 1200, 'DoDelete', '单选删除', null, null, '/Api/CoreCmsAgentArea/DoDelete', 1, 6, 'CoreCmsAgentArea:DoDelete', null, null, '0', '0', NOW(), null);

-- 批量删除
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1209, 1200, 'DoBatchDelete', '批量删除', null, null, '/Api/CoreCmsAgentArea/DoBatchDelete', 1, 7, 'CoreCmsAgentArea:DoBatchDelete', null, null, '0', '0', NOW(), null);

-- 预览数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1210, 1200, 'GetDetails', '预览数据', null, null, '/Api/CoreCmsAgentArea/GetDetails', 1, 8, 'CoreCmsAgentArea:GetDetails', null, null, '0', '0', NOW(), null);

-- 设置启用状态
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1211, 1200, 'SetEnable', '设置启用状态', null, null, '/Api/CoreCmsAgentArea/SetEnable', 1, 9, 'CoreCmsAgentArea:SetEnable', null, null, '0', '0', NOW(), null);

-- 获取代理商列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1212, 1200, 'GetAgentList', '获取代理商列表', null, null, '/Api/CoreCmsAgentArea/GetAgentList', 1, 10, 'CoreCmsAgentArea:GetAgentList', null, null, '0', '0', NOW(), null);

-- 获取地区树
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1213, 1200, 'GetAreaTree', '获取地区树', null, null, '/Api/CoreCmsAgentArea/GetAreaTree', 1, 11, 'CoreCmsAgentArea:GetAreaTree', null, null, '0', '0', NOW(), null);

-- 获取省份列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1214, 1200, 'GetProvinceList', '获取省份列表', null, null, '/Api/CoreCmsAgentArea/GetProvinceList', 1, 12, 'CoreCmsAgentArea:GetProvinceList', null, null, '0', '0', NOW(), null);

-- 获取城市列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1215, 1200, 'GetCityList', '获取城市列表', null, null, '/Api/CoreCmsAgentArea/GetCityList', 1, 13, 'CoreCmsAgentArea:GetCityList', null, null, '0', '0', NOW(), null);

-- 获取区县列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1216, 1200, 'GetCountyList', '获取区县列表', null, null, '/Api/CoreCmsAgentArea/GetCountyList', 1, 14, 'CoreCmsAgentArea:GetCountyList', null, null, '0', '0', NOW(), null);

-- 4. 地区代理订单记录相关API权限菜单
-- 获取列表
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1217, 1201, 'GetPageList', '获取列表', null, null, '/Api/CoreCmsAgentAreaOrder/GetPageList', 1, 0, 'CoreCmsAgentAreaOrder:GetPageList', null, null, '0', '0', NOW(), null);

-- 首页数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1218, 1201, 'GetIndex', '首页数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetIndex', 1, 1, 'CoreCmsAgentAreaOrder:GetIndex', null, null, '0', '0', NOW(), null);

-- 预览数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1219, 1201, 'GetDetails', '预览数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetDetails', 1, 2, 'CoreCmsAgentAreaOrder:GetDetails', null, null, '0', '0', NOW(), null);

-- 编辑提交
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1220, 1201, 'DoEdit', '编辑提交', null, null, '/Api/CoreCmsAgentAreaOrder/DoEdit', 1, 3, 'CoreCmsAgentAreaOrder:DoEdit', null, null, '0', '0', NOW(), null);

-- 获取统计数据
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1221, 1201, 'GetStatistics', '获取统计数据', null, null, '/Api/CoreCmsAgentAreaOrder/GetStatistics', 1, 4, 'CoreCmsAgentAreaOrder:GetStatistics', null, null, '0', '0', NOW(), null);

-- 结算操作
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1222, 1201, 'DoSettle', '结算操作', null, null, '/Api/CoreCmsAgentAreaOrder/DoSettle', 1, 5, 'CoreCmsAgentAreaOrder:DoSettle', null, null, '0', '0', NOW(), null);

-- 单个订单结算
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1223, 1201, 'DoSettleOrder', '单个订单结算', null, null, '/Api/CoreCmsAgentAreaOrder/DoSettleOrder', 1, 6, 'CoreCmsAgentAreaOrder:DoSettleOrder', null, null, '0', '0', NOW(), null);

-- 批量结算
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1224, 1201, 'DoBatchSettle', '批量结算', null, null, '/Api/CoreCmsAgentAreaOrder/DoBatchSettle', 1, 7, 'CoreCmsAgentAreaOrder:DoBatchSettle', null, null, '0', '0', NOW(), null);

-- 设置结算状态
INSERT INTO `SysMenu` (`id`, `parentId`, `identificationCode`, `menuName`, `menuIcon`, `path`, `component`, `menuType`, `sortNumber`, `authority`, `target`, `iconColor`, `hide`, `deleted`, `createTime`, `updateTime`) 
VALUES (1225, 1201, 'DoSetSettleStatus', '设置结算状态', null, null, '/Api/CoreCmsAgentAreaOrder/DoSetSettleStatus', 1, 8, 'CoreCmsAgentAreaOrder:DoSetSettleStatus', null, null, '0', '0', NOW(), null);