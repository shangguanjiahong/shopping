using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace CoreCms.Net.Services
{
    /// <summary>
    /// VIP会员服务实现
    /// </summary>
    public class CoreCmsUserVipServices : BaseServices<CoreCmsUserVip>, ICoreCmsUserVipServices
    {
        private readonly ICoreCmsUserVipRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreCmsUserServices _userServices;

        public CoreCmsUserVipServices(ICoreCmsUserVipRepository dal, IUnitOfWork unitOfWork, ICoreCmsUserServices userServices)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
            _userServices = userServices;
        }

        /// <summary>
        /// 创建VIP订单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="vipLevel">VIP等级</param>
        /// <param name="payAmount">支付金额</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CreateVipOrder(int userId, int vipLevel, decimal payAmount)
        {
            var jm = new WebApiCallBack();

            try
            {
                var vipOrder = new CoreCmsUserVip
                {
                    userId = userId,
                    vipLevel = vipLevel,
                    payAmount = payAmount,
                    payOrderId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.支付单编号),
                    status = 2, // 待支付
                    createTime = DateTime.Now,
                    updateTime = DateTime.Now
                };

                var result = await _dal.InsertAsync(vipOrder);
                if (result != null)
                {
                    jm.status = true;
                    jm.msg = "VIP订单创建成功";
                    jm.data = new { payOrderId = vipOrder.payOrderId };
                }
                else
                {
                    jm.msg = "VIP订单创建失败";
                }
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message;
            }

            return jm;
        }

        /// <summary>
        /// 获取用户VIP信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetUserVipInfo(int userId)
        {
            var jm = new WebApiCallBack();

            try
            {
                var vipInfo = await _dal.QueryByClauseAsync(p => p.userId == userId && p.status == 1);
                if (vipInfo != null)
                {
                    jm.status = true;
                    jm.msg = "获取VIP信息成功";
                    jm.data = new { vipLevel = vipInfo.vipLevel, status = vipInfo.status };
                }
                else
                {
                    jm.msg = "用户暂无VIP信息";
                }
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message;
            }

            return jm;
        }

        /// <summary>
        /// 获取VIP订单状态
        /// </summary>
        /// <param name="payOrderId">支付订单号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetVipOrderStatus(string payOrderId)
        {
            var jm = new WebApiCallBack();

            try
            {
                var vipOrder = await _dal.QueryByClauseAsync(p => p.payOrderId == payOrderId);
                if (vipOrder != null)
                {
                    jm.status = true;
                    jm.msg = "获取订单状态成功";
                    jm.data = new { 
                        status = vipOrder.status, 
                        vipLevel = vipOrder.vipLevel,
                        payAmount = vipOrder.payAmount  // 添加支付金额字段
                    };
                }
                else
                {
                    jm.msg = "订单不存在";
                }
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message;
            }

            return jm;
        }

        /// <summary>
        /// VIP支付成功处理
        /// </summary>
        /// <param name="payOrderId">支付订单号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> VipPaySuccess(string payOrderId)
        {
            var jm = new WebApiCallBack();

            try
            {
                var vipOrder = await _dal.QueryByClauseAsync(p => p.payOrderId == payOrderId && p.status == 2);
                if (vipOrder != null)
                {
                    // 更新VIP状态为有效
                    vipOrder.status = 1;
                    vipOrder.updateTime = DateTime.Now;
                    
                    await _dal.UpdateAsync(vipOrder);
                    
                    // 更新用户等级为VIP等级 - 显式指定属性访问
                    var userInfo = await _userServices.QueryByIdAsync(vipOrder.userId);
                    if (userInfo != null)
                    {
                        userInfo.grade = vipOrder.vipLevel;
                        userInfo.updataTime = DateTime.Now;
                        await _userServices.UpdateAsync(userInfo);
                    }
                    
                    jm.status = true;
                    jm.msg = "VIP开通成功";
                }
                else
                {
                    jm.msg = "订单不存在或已处理";
                }
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message;
            }

            return jm;
        }

        /// <summary>
        /// 检查用户VIP状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<bool> CheckUserVipStatus(int userId)
        {
            try
            {
                var vipInfo = await _dal.QueryByClauseAsync(p => p.userId == userId && p.status == 1);
                return vipInfo != null;
            }
            catch
            {
                return false;
            }
        }
    }
}