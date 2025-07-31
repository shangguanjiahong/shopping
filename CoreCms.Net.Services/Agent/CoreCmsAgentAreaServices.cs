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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.ViewModels.UI;

using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    /// 代理商地区绑定表 接口实现
    /// </summary>
    public class CoreCmsAgentAreaServices : BaseServices<CoreCmsAgentArea>, ICoreCmsAgentAreaServices
    {
        private readonly ICoreCmsAgentAreaRepository _dal;
        private readonly ICoreCmsAgentRepository _agentRepository;
        private readonly ICoreCmsAreaRepository _areaRepository;

        public CoreCmsAgentAreaServices(ICoreCmsAgentAreaRepository dal, ICoreCmsAgentRepository agentRepository, ICoreCmsAreaRepository areaRepository)
        {
            _dal = dal;
            _agentRepository = agentRepository;
            _areaRepository = areaRepository;
            BaseDal = dal;
        }

        #region 重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentArea entity)
        {
            var jm = new AdminUiCallBack();

            // 验证代理商是否存在
            var agent = await _agentRepository.QueryByIdAsync(entity.agentId);
            if (agent == null)
            {
                jm.msg = "代理商不存在";
                return jm;
            }

            // 验证地区是否存在
            var area = await _areaRepository.QueryByIdAsync(entity.areaId);
            if (area == null)
            {
                jm.msg = "地区不存在";
                return jm;
            }

            // 验证地区深度是否匹配
            if (area.depth != entity.areaDepth)
            {
                jm.msg = "地区深度不匹配";
                return jm;
            }

            // 检查代理商是否已绑定其他地区
            var existingBinding = await _dal.QueryListByClauseAsync(p => p.agentId == entity.agentId && p.isDelete == false);
            if (existingBinding.Any())
            {
                jm.msg = "该代理商已绑定其他地区";
                return jm;
            }

            // 检查该地区是否已有代理商
            var hasAgent = await _dal.CheckAreaHasAgentAsync(entity.areaId, entity.areaDepth);
            if (hasAgent)
            {
                jm.msg = "该地区已有代理商";
                return jm;
            }

            // 设置省市县ID
            await SetAreaHierarchy(entity, area);


            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentArea entity)
        {
            var jm = new AdminUiCallBack();

            // 验证代理商是否存在
            var agent = await _agentRepository.QueryByIdAsync(entity.agentId);
            if (agent == null)
            {
                jm.msg = "代理商不存在";
                return jm;
            }

            // 验证地区是否存在
            var area = await _areaRepository.QueryByIdAsync(entity.areaId);
            if (area == null)
            {
                jm.msg = "地区不存在";
                return jm;
            }

            // 验证地区深度是否匹配
            if (area.depth != entity.areaDepth)
            {
                jm.msg = "地区深度不匹配";
                return jm;
            }

            // 检查该地区是否已有其他代理商
            var hasAgent = await _dal.CheckAreaHasAgentAsync(entity.areaId, entity.areaDepth, entity.agentId);
            if (hasAgent)
            {
                jm.msg = "该地区已有其他代理商";
                return jm;
            }

            // 设置省市县ID
            await SetAreaHierarchy(entity, area);

            var bl = await _dal.UpdateAsync(entity);
            if (bl.code == 0)
            {
                await _dal.UpdateCaChe();
            }
            return bl;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentArea> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var bl = await _dal.DeleteByIdAsync(id);
            if (bl.code == 0)
            {
                await _dal.UpdateCaChe();
            }
            return bl;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var bl = await _dal.DeleteByIdsAsync(ids);
            if (bl.code == 0)
            {
                await _dal.UpdateCaChe();
            }
            return bl;
        }

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsAgentArea>> GetCaChe()
        {
            return await _dal.GetCaChe();
        }

        /// <summary>
        /// 更新cache
        /// </summary>
        public async Task<List<CoreCmsAgentArea>> UpdateCaChe()
        {
            return await _dal.UpdateCaChe();
        }

        #endregion

        #region 扩展方法==========================================================

        /// <summary>
        /// 根据地区ID获取代理商信息
        /// </summary>
        /// <param name="areaId">地区ID</param>
        /// <param name="areaDepth">地区深度</param>
        /// <returns></returns>
        public async Task<CoreCmsAgentArea> GetAgentByAreaAsync(int areaId, int areaDepth)
        {
            return await _dal.GetAgentByAreaAsync(areaId, areaDepth);
        }

        /// <summary>
        /// 根据省市县获取对应的代理商
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <param name="countyId">县ID</param>
        /// <returns></returns>
        public async Task<List<CoreCmsAgentArea>> GetAgentsByAreaHierarchyAsync(int? provinceId, int? cityId, int? countyId)
        {
            return await _dal.GetAgentsByAreaHierarchyAsync(provinceId, cityId, countyId);
        }

        /// <summary>
        /// 检查地区是否已有代理商
        /// </summary>
        /// <param name="areaId">地区ID</param>
        /// <param name="areaDepth">地区深度</param>
        /// <param name="excludeAgentId">排除的代理商ID</param>
        /// <returns></returns>
        public async Task<bool> CheckAreaHasAgentAsync(int areaId, int areaDepth, int excludeAgentId = 0)
        {
            return await _dal.CheckAreaHasAgentAsync(areaId, areaDepth, excludeAgentId);
        }

        /// <summary>
        /// 根据省市县获取对应的代理商（优先级：县 > 市 > 省）
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <param name="countyId">县ID</param>
        /// <returns></returns>
        public async Task<CoreCmsAgentArea> GetAgentByArea(int? provinceId, int? cityId, int? countyId)
        {
            var agentAreas = await GetAgentsByAreaHierarchyAsync(provinceId, cityId, countyId);

            // 优先级：县 > 市 > 省
            if (countyId.HasValue)
            {
                var countyAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.County && x.countyId == countyId);
                if (countyAgent != null) return countyAgent;
            }

            if (cityId.HasValue)
            {
                var cityAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.City && x.cityId == cityId);
                if (cityAgent != null) return cityAgent;
            }

            if (provinceId.HasValue)
            {
                var provinceAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.Province && x.provinceId == provinceId);
                if (provinceAgent != null) return provinceAgent;
            }

            return null;

        }

        /// <summary>
        /// 根据省市县获取所有层级的代理商列表
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <param name="countyId">县ID</param>
        /// <returns></returns>
        public async Task<List<CoreCmsAgentArea>> GetAllLevelAgentsByArea(int? provinceId, int? cityId, int? countyId)
        {
            var agentAreas = await GetAgentsByAreaHierarchyAsync(provinceId, cityId, countyId);
            var result = new List<CoreCmsAgentArea>();

            // 按层级顺序添加：县 > 市 > 省
            if (countyId.HasValue)
            {
                var countyAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.County && x.countyId == countyId);
                if (countyAgent != null) result.Add(countyAgent);
            }

            if (cityId.HasValue)
            {
                var cityAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.City && x.cityId == cityId);
                if (cityAgent != null) result.Add(cityAgent);
            }

            if (provinceId.HasValue)
            {
                var provinceAgent = agentAreas.FirstOrDefault(x => x.areaDepth == (int)GlobalEnumVars.AreaDepth.Province && x.provinceId == provinceId);
                if (provinceAgent != null) result.Add(provinceAgent);
            }

            return result;

        }

        /// <summary>
        /// 设置地区层级信息
        /// </summary>
        /// <param name="entity">代理地区绑定实体</param>
        /// <param name="area">地区信息</param>
        /// <returns></returns>
        private async Task SetAreaHierarchy(CoreCmsAgentArea entity, CoreCmsArea area)
        {
            entity.provinceId = 0;
            entity.cityId = 0;
            entity.countyId = 0;

            if (area.depth == (int)GlobalEnumVars.AreaDepth.Province)
            {
                entity.provinceId = area.id;
            }
            else if (area.depth == (int)GlobalEnumVars.AreaDepth.City)
            {
                entity.cityId = area.id;
                var province = await _areaRepository.QueryByClauseAsync(p => p.id == area.parentId);
                if (province != null) entity.provinceId = province.id;
            }
            else if (area.depth == (int)GlobalEnumVars.AreaDepth.County)
            {
                entity.countyId = area.id;
                var city = await _areaRepository.QueryByClauseAsync(p => p.id == area.parentId);
                if (city != null)
                {
                    entity.cityId = city.id;
                    var province = await _areaRepository.QueryByClauseAsync(p => p.id == city.parentId);
                    if (province != null) entity.provinceId = province.id;
                }
            }
        }

        /// <summary>
        /// 获取代理商ID列表
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <param name="countyId">县ID</param>
        /// <returns></returns>
        public async Task<List<int>> GetAgentIds(int? provinceId, int? cityId, int? countyId)
        {
            var agentAreas = await GetAllLevelAgentsByArea(provinceId, cityId, countyId);
            return agentAreas.Select(x => x.agentId).Distinct().ToList();
        }

        #endregion
    }
}