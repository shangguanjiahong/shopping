/**
 * 地区代理管理模块
 * 用于处理地区代理相关的前端逻辑
 */

layui.define(['table', 'form', 'layer'], function(exports) {
    var $ = layui.$,
        admin = layui.admin,
        table = layui.table,
        form = layui.form,
        layer = layui.layer;

    var obj = {
        // 初始化
        init: function() {
            console.log('地区代理管理模块已加载');
        },
        
        // 刷新表格
        reload: function() {
            table.reload('LAY-app-CoreCmsAgentArea-tableBox');
        },
        
        // 其他功能可以在这里扩展
        utils: {
            // 工具函数
        }
    };

    // 导出模块
    exports('agentAreaAdmin', obj);
});