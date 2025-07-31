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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 代理商地区绑定表 接口实现
    /// </summary>
    public class CoreCmsAgentAreaRepository : BaseRepository<CoreCmsAgentArea>, ICoreCmsAgentAreaRepository
    {
        public CoreCmsAgentAreaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentArea entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentArea entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsAgentArea>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //如果地区和原来不一样，就需要校验
            if (entity.areaId != oldModel.areaId)
            {
                if (await CheckAreaHasAgentAsync(entity.areaId, entity.areaDepth, entity.agentId))
                {
                    jm.msg = "该地区已被其他代理商代理";
                    return jm;
                }
            }

            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.agentId = entity.agentId;
            oldModel.areaId = entity.areaId;
            oldModel.areaDepth = entity.areaDepth;
            oldModel.provinceId = entity.provinceId;
            oldModel.cityId = entity.cityId;
            oldModel.countyId = entity.countyId;
            oldModel.commissionRate = entity.commissionRate;
            oldModel.remark = entity.remark;
            oldModel.isEnable = entity.isEnable;
            oldModel.isDelete = entity.isDelete;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentArea> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsAgentArea>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsAgentArea>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsAgentArea>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsAgentArea>>(GlobalConstVars.CacheCoreCmsAgentArea);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        /// 更新cache
        /// </summary>
        public async Task<List<CoreCmsAgentArea>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<CoreCmsAgentArea>()
                .Where(p => p.isDelete == false)
                .OrderBy(p => p.areaDepth)
                .OrderBy(p => p.createTime)
                .With(SqlWith.NoLock)
                .ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsAgentArea, list);
            return list;
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
            return await DbClient.Queryable<CoreCmsAgentArea>()
                .Where(p => p.areaId == areaId && p.areaDepth == areaDepth && p.isDelete == false)
                .FirstAsync();
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
            var query = DbClient.Queryable<CoreCmsAgentArea>().Where(p => p.isDelete == false);

            if (provinceId.HasValue)
            {
                query = query.Where(p => p.provinceId == provinceId.Value);
            }
            if (cityId.HasValue)
            {
                query = query.Where(p => p.cityId == cityId.Value);
            }
            if (countyId.HasValue)
            {
                query = query.Where(p => p.countyId == countyId.Value);
            }

            return await query.ToListAsync();
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
            var query = DbClient.Queryable<CoreCmsAgentArea>()
                .Where(p => p.areaId == areaId && p.areaDepth == areaDepth && p.isDelete == false);

            if (excludeAgentId > 0)
            {
                query = query.Where(p => p.agentId != excludeAgentId);
            }

            return await query.AnyAsync();
        }


        #endregion
    }
}