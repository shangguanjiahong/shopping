/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     代理商地区绑定表 工厂接口
    /// </summary>
    public interface ICoreCmsAgentAreaRepository : IBaseRepository<CoreCmsAgentArea>
    {
        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsAgentArea entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentArea entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentArea> entity);

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdAsync(object id);

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        ///     获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<CoreCmsAgentArea>> GetCaChe();

        /// <summary>
        ///     更新cache
        /// </summary>
        Task<List<CoreCmsAgentArea>> UpdateCaChe();

        #endregion

        #region 扩展方法==========================================================

        /// <summary>
        ///     根据地区ID获取代理商信息
        /// </summary>
        /// <param name="areaId">地区ID</param>
        /// <param name="areaDepth">地区深度</param>
        /// <returns></returns>
        Task<CoreCmsAgentArea> GetAgentByAreaAsync(int areaId, int areaDepth);

        /// <summary>
        ///     根据省市县获取对应的代理商
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <param name="countyId">县ID</param>
        /// <returns></returns>
        Task<List<CoreCmsAgentArea>> GetAgentsByAreaHierarchyAsync(int? provinceId, int? cityId, int? countyId);

        /// <summary>
        ///     检查地区是否已有代理商
        /// </summary>
        /// <param name="areaId">地区ID</param>
        /// <param name="areaDepth">地区深度</param>
        /// <param name="excludeAgentId">排除的代理商ID</param>
        /// <returns></returns>
        Task<bool> CheckAreaHasAgentAsync(int areaId, int areaDepth, int excludeAgentId = 0);

        #endregion
    }
}