/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: VIP会员服务接口
***********************************************************************/ 
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using System.Threading.Tasks;

namespace CoreCms.Net.IServices
{
    /// <summary>
    /// VIP会员服务接口
    /// </summary>
    public interface ICoreCmsUserVipServices : IBaseServices<CoreCmsUserVip>
    {
        /// <summary>
        /// 创建VIP订单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="vipLevel">VIP等级</param>
        /// <param name="payAmount">支付金额</param>
        /// <returns></returns>
        Task<WebApiCallBack> CreateVipOrder(int userId, int vipLevel, decimal payAmount);

        /// <summary>
        /// 获取用户VIP信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetUserVipInfo(int userId);

        /// <summary>
        /// 获取VIP订单状态
        /// </summary>
        /// <param name="payOrderId">支付订单号</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetVipOrderStatus(string payOrderId);

        /// <summary>
        /// VIP支付成功处理
        /// </summary>
        /// <param name="payOrderId">支付订单号</param>
        /// <returns></returns>
        Task<WebApiCallBack> VipPaySuccess(string payOrderId);

        /// <summary>
        /// 检查用户VIP状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<bool> CheckUserVipStatus(int userId);
    }
}