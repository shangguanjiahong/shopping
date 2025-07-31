/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Web.Admin.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 地区代理订单记录表
    ///</summary>
    [Description("地区代理订单记录表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsAgentAreaOrderController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsAgentAreaOrderServices _coreCmsAgentAreaOrderServices;
        private readonly ICoreCmsAgentAreaServices _coreCmsAgentAreaServices;
        private readonly ICoreCmsOrderServices _coreCmsOrderServices;
        private readonly ICoreCmsUserServices _coreCmsUserServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsAgentAreaOrderController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsAgentAreaOrderServices coreCmsAgentAreaOrderServices
            , ICoreCmsAgentAreaServices coreCmsAgentAreaServices
            , ICoreCmsOrderServices coreCmsOrderServices
            , ICoreCmsUserServices coreCmsUserServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsAgentAreaOrderServices = coreCmsAgentAreaOrderServices;
            _coreCmsAgentAreaServices = coreCmsAgentAreaServices;
            _coreCmsOrderServices = coreCmsOrderServices;
            _coreCmsUserServices = coreCmsUserServices;
        }

        #region 获取列表============================================================
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsAgentOrder>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsAgentOrder, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "agentId":
                    orderEx = p => p.agentId;
                    break;
                case "buyUserId":
                    orderEx = p => p.buyUserId;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "money":
                    orderEx = p => p.amount;
                    break;
                case "settleStatus":
                    orderEx = p => p.isSettlement;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }
            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //代理商ID int
            var agentId = Request.Form["agentId"].FirstOrDefault().ObjectToInt(0);
            if (agentId > 0)
            {
                where = where.And(p => p.agentId == agentId);
            }
            //购买用户ID int
            var buyUserId = Request.Form["buyUserId"].FirstOrDefault().ObjectToInt(0);
            if (buyUserId > 0)
            {
                where = where.And(p => p.buyUserId == buyUserId);
            }
            //订单ID nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId))
            {
                where = where.And(p => p.orderId.Contains(orderId));
            }
            //结算状态 int
            var settleStatus = Request.Form["settleStatus"].FirstOrDefault().ObjectToInt(-1);
            if (settleStatus > -1)
            {
                where = where.And(p => p.isSettlement == settleStatus);
            }
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }
            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                var dt = updateTime.ObjectToDate();
                where = where.And(p => p.updateTime > dt);
            }
            //获取数据
            var list = await _coreCmsAgentAreaOrderServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsAgentAreaOrderServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 设置结算状态============================================================
        /// <summary>
        /// 设置结算状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置结算状态")]
        public async Task<AdminUiCallBack> DoSetSettleStatus([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsAgentAreaOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isSettlement = entity.data ? 1 : 0;
            oldModel.updateTime = DateTime.Now;

            jm = await _coreCmsAgentAreaOrderServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion

        // POST: Api/CoreCmsAgentAreaOrder/GetStatistics
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取统计信息")]
        public async Task<JsonResult> GetStatistics()
        {
            var jm = new AdminUiCallBack();

            var agentAreaId = Request.Form["agentAreaId"].FirstOrDefault().ObjectToInt(0);
            if (agentAreaId <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            var orders = await _coreCmsAgentAreaOrderServices.QueryListByClauseAsync(p => p.agentAreaId == agentAreaId);
            
            var statistics = new
            {
                totalOrders = orders.Count,
                totalCommission = orders.Sum(p => p.commissionAmount),
                settledCommission = orders.Where(p => p.isSettled).Sum(p => p.commissionAmount),
                unsettledCommission = orders.Where(p => !p.isSettled).Sum(p => p.commissionAmount)
            };

            jm.code = 0;
            jm.data = statistics;
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentAreaOrder/DoSettle
        /// <summary>
        /// 批量结算代理商订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("批量结算代理商订单")]
        public async Task<JsonResult> DoSettle()
        {
            var jm = new AdminUiCallBack();

            var agentAreaId = Request.Form["agentAreaId"].FirstOrDefault().ObjectToInt(0);
            if (agentAreaId <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            // 获取未结算的订单
            var unsettledOrders = await _coreCmsAgentAreaOrderServices.QueryListByClauseAsync(p => p.agentAreaId == agentAreaId && !p.isSettled);
            
            if (!unsettledOrders.Any())
            {
                jm.msg = "没有需要结算的订单";
                return new JsonResult(jm);
            }

            // 批量更新结算状态
            foreach (var order in unsettledOrders)
            {
                order.isSettled = true;
                order.settleTime = DateTime.Now;
                order.updateTime = DateTime.Now;
            }

            var result = await _coreCmsAgentAreaOrderServices.UpdateAsync(unsettledOrders);
            if (result.code == 0)
            {
                jm.code = 0;
                jm.msg = $"成功结算 {unsettledOrders.Count} 个订单";
            }
            else
            {
                jm.msg = result.msg;
            }

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentAreaOrder/DoSettleOrder
        /// <summary>
        /// 结算单个订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("结算单个订单")]
        public async Task<JsonResult> DoSettleOrder()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            var model = await _coreCmsAgentAreaOrderServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "订单不存在";
                return new JsonResult(jm);
            }

            if (model.isSettled)
            {
                jm.msg = "订单已结算";
                return new JsonResult(jm);
            }

            model.isSettled = true;
            model.settleTime = DateTime.Now;
            model.updateTime = DateTime.Now;

            jm = await _coreCmsAgentAreaOrderServices.UpdateAsync(model);
            if (jm.code == 0)
            {
                jm.msg = "结算成功";
            }

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentAreaOrder/BatchSettle
        /// <summary>
        /// 批量结算选中订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("批量结算选中订单")]
        public async Task<JsonResult> BatchSettle()
        {
            var jm = new AdminUiCallBack();

            var ids = Request.Form["ids"].FirstOrDefault();
            if (string.IsNullOrEmpty(ids))
            {
                jm.msg = "请选择要结算的订单";
                return new JsonResult(jm);
            }

            var idArray = ids.Split(',').Select(int.Parse).ToArray();
            var orders = await _coreCmsAgentAreaOrderServices.QueryListByClauseAsync(p => idArray.Contains(p.id) && !p.isSettled);
            
            if (!orders.Any())
            {
                jm.msg = "没有需要结算的订单";
                return new JsonResult(jm);
            }

            // 批量更新结算状态
            foreach (var order in orders)
            {
                order.isSettled = true;
                order.settleTime = DateTime.Now;
                order.updateTime = DateTime.Now;
            }

            var result = await _coreCmsAgentAreaOrderServices.UpdateAsync(orders);
            if (result.code == 0)
            {
                jm.code = 0;
                jm.msg = $"成功结算 {orders.Count} 个订单";
            }
            else
            {
                jm.msg = result.msg;
            }

            return new JsonResult(jm);
        }

    }
}