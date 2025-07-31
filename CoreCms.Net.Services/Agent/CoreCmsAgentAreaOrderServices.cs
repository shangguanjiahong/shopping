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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     代理商地区订单记录表 接口实现
    /// </summary>
    public class CoreCmsAgentAreaOrderServices : BaseServices<CoreCmsAgentOrder>, ICoreCmsAgentAreaOrderServices
    {
        private readonly ICoreCmsAgentGoodsServices _agentGoodsServices;
        private readonly ICoreCmsAgentProductsServices _agentProductsServices;
        private readonly ICoreCmsUserBalanceServices _balanceServices;
        private readonly ICoreCmsAgentOrderRepository _dal;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsAgentAreaServices _agentAreaServices;
        private readonly ICoreCmsAgentGradeServices _agentGradeServices;

        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICoreCmsUserServices _userServices;


        public CoreCmsAgentAreaOrderServices(IUnitOfWork unitOfWork, ICoreCmsAgentOrderRepository dal,
            ICoreCmsUserServices userServices, ICoreCmsOrderItemServices orderItemServices,
            ICoreCmsProductsServices productsServices, ICoreCmsGoodsServices goodsServices,
            ICoreCmsAgentProductsServices agentProductsServices, ICoreCmsSettingServices settingServices,
            ICoreCmsAgentGoodsServices agentGoodsServices, IServiceProvider serviceProvider,
            ICoreCmsOrderServices orderServices, ICoreCmsUserBalanceServices balanceServices,
            ICoreCmsAreaServices areaServices, ICoreCmsAgentAreaServices agentAreaServices,
            ICoreCmsAgentGradeServices agentGradeServices)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
            _userServices = userServices;
            _orderItemServices = orderItemServices;
            _productsServices = productsServices;
            _goodsServices = goodsServices;
            _agentProductsServices = agentProductsServices;
            _settingServices = settingServices;
            _agentGoodsServices = agentGoodsServices;
            _serviceProvider = serviceProvider;
            _orderServices = orderServices;
            _balanceServices = balanceServices;
            _areaServices = areaServices;
            _agentAreaServices = agentAreaServices;
            _agentGradeServices = agentGradeServices;
        }

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentOrder entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentOrder entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentOrder> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion

        #region 重写根据条件查询分页数据

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsAgentOrder>> QueryPageAsync(
            Expression<Func<CoreCmsAgentOrder, bool>> predicate,
            Expression<Func<CoreCmsAgentOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
                blUseNoLock);
        }

        #endregion

        #region 添加代理地区订单关联记录

        /// <summary>
        ///     添加代理地区订单关联记录
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(CoreCmsOrder order)
        {
            var jm = new WebApiCallBack();

            try
            {
                // 根据订单收货地址获取省市区信息
                var areaInfo = await _areaServices.QueryByIdAsync(order.shipAreaId);
                if (areaInfo == null)
                {
                    jm.status = false;
                    jm.msg = "订单收货地址信息不存在";
                    return jm;
                }

                // 获取省市区的完整层级信息
                var provinceId = 0;
                var cityId = 0;
                var countyId = 0;

                if (areaInfo.depth == (int)GlobalEnumVars.AreaDepth.County)
                {
                    countyId = areaInfo.id;
                    var cityInfo = await _areaServices.QueryByIdAsync(areaInfo.parentId);
                    if (cityInfo != null)
                    {
                        cityId = cityInfo.id;
                        var provinceInfo = await _areaServices.QueryByIdAsync(cityInfo.parentId);
                        if (provinceInfo != null)
                        {
                            provinceId = provinceInfo.id;
                        }
                    }
                }
                else if (areaInfo.depth == (int)GlobalEnumVars.AreaDepth.City)
                {
                    cityId = areaInfo.id;
                    var provinceInfo = await _areaServices.QueryByIdAsync(areaInfo.parentId);
                    if (provinceInfo != null)
                    {
                        provinceId = provinceInfo.id;
                    }
                }
                else if (areaInfo.depth == (int)GlobalEnumVars.AreaDepth.Province)
                {
                    provinceId = areaInfo.id;
                }

                // 根据省市区查找所有层级的代理商
                var agentAreas = await _agentAreaServices.GetAllLevelAgentsByArea(provinceId, cityId, countyId);
                if (!agentAreas.Any())
                {
                    jm.status = false;
                    jm.msg = "该地区暂无代理商";
                    return jm;
                }

                // 获取购物明细（所有代理商共用）
                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                var goodIds = orderItems.Select(p => p.goodsId).ToList();
                var productIds = orderItems.Select(p => p.productId).ToList();
                
                // 获取商品数据
                var goods = await _goodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.id));
                // 获取货品数据
                var products = await _productsServices.QueryListByClauseAsync(p => productIds.Contains(p.id));
                // 获取当前订单包含的商品在代理商货品池启用商品数据
                var agentGoods = await _agentGoodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.goodId) && p.isEnable);

                using var container = _serviceProvider.CreateScope();
                var agentServices = container.ServiceProvider.GetService<ICoreCmsAgentServices>();
                
                var successCount = 0;
                var errorMessages = new List<string>();

                // 为每个层级的代理商进行结算
                foreach (var agentArea in agentAreas)
                {
                    try
                    {
                        // 获取代理商信息
                        var agentModel = await agentServices.QueryByIdAsync(agentArea.agentId);
                        if (agentModel == null || agentModel.verifyStatus != (int)GlobalEnumVars.AgentVerifyStatus.VerifyYes)
                        {
                            errorMessages.Add($"代理商ID:{agentArea.agentId} 信息不存在或未通过审核");
                            continue;
                        }

                        // 获取代理商等级信息
                        var agentGrade = await _agentGradeServices.QueryByIdAsync(agentModel.gradeId);
                        if (agentGrade == null)
                        {
                            errorMessages.Add($"代理商ID:{agentArea.agentId} 等级信息不存在");
                            continue;
                        }

                        // 获取货品关联的分销数据（根据代理商等级）
                        var agentProducts = await _agentProductsServices.QueryListByClauseAsync(p => productIds.Contains(p.productId) && p.agentGradeId == agentModel.gradeId);

                        if (agentGoods.Any() && agentProducts.Any())
                        {
                            await AddOther(order, orderItems, goods, products, agentGoods, agentProducts, agentModel, agentArea);
                            successCount++;
                        }
                        else
                        {
                            errorMessages.Add($"代理商ID:{agentArea.agentId} 商品池或货品池为空");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add($"代理商ID:{agentArea.agentId} 处理失败：{ex.Message}");
                    }
                }

                if (successCount > 0)
                {
                    jm.status = true;
                    jm.msg = $"成功为{successCount}个代理商添加订单记录";
                    if (errorMessages.Any())
                    {
                        jm.msg += $"，部分失败：{string.Join(";", errorMessages)}";
                    }
                }
                else
                {
                    jm.status = false;
                    jm.msg = $"所有代理商处理失败：{string.Join(";", errorMessages)}";
                }
            }
            catch (Exception ex)
            {
                jm.status = false;
                jm.msg = $"添加代理地区订单记录失败：{ex.Message}";
            }

            return jm;
        }

        #endregion

        #region 订单结算处理事件

        /// <summary>
        ///     订单结算处理事件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> FinishOrder(string orderId)
        {
            var jm = new WebApiCallBack();

            var order = await _orderServices.QueryByClauseAsync(p =>
                p.orderId == orderId && p.status == (int)GlobalEnumVars.OrderStatus.Complete);
            if (order == null)
            {
                jm.msg = "订单查询失败";
                return jm;
            }

            //更新
            var list = await _dal.QueryListByClauseAsync(p =>
                p.orderId == orderId && p.isSettlement == (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    //钱挪到会员余额里面
                    var result = await _balanceServices.Change(item.userId,
                        (int)GlobalEnumVars.UserBalanceSourceTypes.Agent,
                        item.amount, item.orderId);
                    if (!result.status)
                    {
                        // 记录失败日志
                    }
                }

                await _dal.UpdateAsync(
                    p => new CoreCmsAgentOrder
                    {
                        isSettlement = (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementYes,
                        updateTime = DateTime.Now
                    },
                    p => p.orderId == orderId &&
                         p.isSettlement == (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo);
            }

            jm.status = true;
            return jm;
        }

        #endregion

        #region 作废订单

        /// <summary>
        ///     作废订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CancleOrderByOrderId(string orderId)
        {
            var jm = new WebApiCallBack();

            var list = await _dal.QueryListByClauseAsync(p => p.orderId == orderId);
            if (list.Any())
            {
                await _dal.UpdateAsync(
                    p => new CoreCmsAgentOrder { isDelete = true, updateTime = DateTime.Now },
                    p => p.orderId == orderId);
            }

            jm.status = true;
            return jm;
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     添加代理地区订单记录
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="orderItems">订单明细</param>
        /// <param name="goods">商品信息</param>
        /// <param name="products">货品信息</param>
        /// <param name="agentGoods">代理商品池</param>
        /// <param name="agentProducts">代理货品池</param>
        /// <param name="agentModel">代理商信息</param>
        /// <param name="agentArea">代理地区信息</param>
        /// <returns></returns>
        private async Task AddOther(CoreCmsOrder order, List<CoreCmsOrderItem> orderItems, List<CoreCmsGoods> goods,
            List<CoreCmsProducts> products, List<CoreCmsAgentGoods> agentGoods,
            List<CoreCmsAgentProducts> agentProducts, CoreCmsAgent agentModel, CoreCmsAgentArea agentArea)
        {
            decimal amount = 0;

            foreach (var item in orderItems)
            {
                // 判断商品是否在代理商品池中
                var agentGood = agentGoods.Find(p => p.goodId == item.goodsId);
                if (agentGood == null) continue;

                // 判断代理商代理池是否包含此货品数据
                var agentProduct = agentProducts.Find(p => p.productId == item.productId);
                if (agentProduct == null) continue;

                // 根据代理地区的佣金比例计算佣金
                var commissionAmount = 0m;
                
                // 获取实际当前单个商品应获得利润（基于代理地区的佣金比例）
                var itemTotalAmount = item.price * item.nums - item.promotionAmount;
                if (itemTotalAmount <= 0) continue;

                // 使用代理地区设置的佣金比例计算
                commissionAmount = Math.Round(itemTotalAmount * agentArea.commissionRate / 100, 2);
                
                if (commissionAmount > 0)
                {
                    amount += commissionAmount;
                }
            }

            if (amount > 0)
            {
                var iData = new CoreCmsAgentOrder();
                iData.userId = agentModel.userId;
                iData.buyUserId = order.userId;
                iData.orderId = order.orderId;
                iData.amount = amount;
                iData.isSettlement = (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo; //默认未结算
                iData.isDelete = false;
                iData.createTime = DateTime.Now;
                iData.updateTime = DateTime.Now;
                
                // 判断是否返利过,有历史记录直接更新
                var agentOrder = await _dal.QueryByClauseAsync(p => p.userId == agentModel.userId && p.orderId == order.orderId);
                if (agentOrder != null)
                {
                    agentOrder.amount = amount;
                    agentOrder.updateTime = DateTime.Now;
                    await _dal.UpdateAsync(agentOrder);
                }
                else
                {
                    await _dal.InsertAsync(iData);
                }
            }
        }

        #endregion
    }
}