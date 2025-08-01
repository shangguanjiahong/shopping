/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: VIP会员控制器
***********************************************************************/ 
using CoreCms.Net.Auth;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// VIP会员控制器
    /// </summary>
    [Description("VIP会员")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserVipController : ControllerBase
    {
        private readonly ICoreCmsUserVipServices _userVipServices;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly IHttpContextUser _user;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userVipServices"></param>
        /// <param name="billPaymentsServices"></param>
        /// <param name="user"></param>
        public UserVipController(ICoreCmsUserVipServices userVipServices, ICoreCmsBillPaymentsServices billPaymentsServices, IHttpContextUser user)
        {
            _userVipServices = userVipServices;
            _billPaymentsServices = billPaymentsServices;
            _user = user;
        }

        /// <summary>
        /// 创建VIP会员支付订单
        /// </summary>
        /// <param name="request">请求参数，包含vipLevel(VIP等级)和amount(支付金额)</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Create([FromBody] dynamic request)
        {
            var jm = new WebApiCallBack();

            if (_user.ID <= 0)
            {
                jm.code = 14007;
                jm.msg = "很抱歉，授权失效，请重新登录!";
                return jm;
            }

            // 从请求中提取参数
            int vipLevel = 1;
            decimal amount = 0;
            
            if (request != null)
            {
                if (request.vipLevel != null)
                {
                    int.TryParse(request.vipLevel.ToString(), out vipLevel);
                }
                if (request.amount != null)
                {
                    decimal.TryParse(request.amount.ToString(), out amount);
                }
            }

            if (amount <= 0)
            {
                jm.msg = "支付金额必须大于0";
                return jm;
            }

            // 创建VIP订单
            var result = await _userVipServices.CreateVipOrder(_user.ID, vipLevel, amount);
            if (result.status)
            {
                // 创建支付记录
                var payOrderId = result.data.GetType().GetProperty("payOrderId")?.GetValue(result.data)?.ToString();
                if (!string.IsNullOrEmpty(payOrderId))
                {
                    var billPayment = new CoreCmsBillPayments
                    {
                        paymentId = payOrderId,
                        sourceId = payOrderId,
                        money = amount,
                        userId = _user.ID,
                        type = (int)GlobalEnumVars.BillPaymentsType.VipOrder,
                        status = (int)GlobalEnumVars.BillPaymentsStatus.NoPay,
                        paymentCode = "pending",
                        ip = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1",
                        parameters = string.Empty,
                        payedMsg = string.Empty,
                        tradeNo = string.Empty,
                        createTime = DateTime.Now
                    };

                    await _billPaymentsServices.InsertAsync(billPayment);
                    
                    // 修改返回数据，添加 orderId 字段以保持前端兼容性
                    result.data = new
                    {
                        payOrderId = payOrderId,
                        orderId = payOrderId, // 添加 orderId 字段
                        amount = amount,
                        vipLevel = vipLevel
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// 获取用户VIP信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetInfo()
        {
            var jm = new WebApiCallBack();

            if (_user.ID <= 0)
            {
                jm.code = 14007;
                jm.msg = "很抱歉，授权失效，请重新登录!";
                return jm;
            }

            var result = await _userVipServices.GetUserVipInfo(_user.ID);
            return result;
        }

        /// <summary>
        /// 获取VIP订单状态
        /// </summary>
        /// <param name="orderId">支付订单号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<WebApiCallBack> GetOrderStatus([FromQuery] string orderId)
        {
            var jm = new WebApiCallBack();

            if (_user.ID <= 0)
            {
                jm.code = 14007;
                jm.msg = "很抱歉，授权失效，请重新登录!";
                return jm;
            }

            if (string.IsNullOrEmpty(orderId))
            {
                jm.msg = "支付订单号不能为空";
                return jm;
            }

            var result = await _userVipServices.GetVipOrderStatus(orderId);
            return result;
        }

        /// <summary>
        /// VIP支付成功回调处理
        /// </summary>
        /// <param name="payOrderId">支付订单号</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<WebApiCallBack> PaySuccess([FromForm] string payOrderId)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(payOrderId))
            {
                jm.msg = "支付订单号不能为空";
                return jm;
            }

            var result = await _userVipServices.VipPaySuccess(payOrderId);
            return result;
        }
    }
}