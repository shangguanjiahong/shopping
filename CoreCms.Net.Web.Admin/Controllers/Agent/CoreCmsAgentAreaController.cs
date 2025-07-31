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
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Web.Admin.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 代理商地区绑定表
    ///</summary>
    [Description("代理商地区绑定表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsAgentAreaController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsAgentAreaServices _coreCmsAgentAreaServices;
        private readonly ICoreCmsAgentServices _coreCmsAgentServices;
        private readonly ICoreCmsAreaServices _coreCmsAreaServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsAgentAreaController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsAgentAreaServices coreCmsAgentAreaServices
            , ICoreCmsAgentServices coreCmsAgentServices
            , ICoreCmsAreaServices coreCmsAreaServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsAgentAreaServices = coreCmsAgentAreaServices;
            _coreCmsAgentServices = coreCmsAgentServices;
            _coreCmsAreaServices = coreCmsAreaServices;
        }

        // POST: Api/CoreCmsAgentArea/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetIndex()
        {
            var jm = new AdminUiCallBack();
            var page = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var limit = Request.Form["limit"].FirstOrDefault().ObjectToInt(10);
            var agentId = Request.Form["agentId"].FirstOrDefault().ObjectToInt(0);
            var areaId = Request.Form["areaId"].FirstOrDefault().ObjectToInt(0);

            var where = PredicateBuilder.True<CoreCmsAgentArea>();
            if (agentId > 0)
            {
                where = where.And(p => p.agentId == agentId);
            }
            if (areaId > 0)
            {
                where = where.And(p => p.areaId == areaId);
            }

            // 使用Join查询
            var totalCount = new RefAsync<int>();
            var list = await _coreCmsAgentAreaServices.Queryable()
                .LeftJoin<CoreCmsAgent>((aa, a) => aa.agentId == a.id)
                .LeftJoin<CoreCmsArea>((aa, a, ar) => aa.areaId == ar.id)
                .Where(where)
                .OrderBy(aa => aa.id, OrderByType.Desc)
                .Select((aa, a, ar) => new
                {
                    id = aa.id,
                    agentId = aa.agentId,
                    agentName = a.name,
                    areaId = aa.areaId,
                    areaName = ar.name,
                    areaDepth = aa.areaDepth,
                    provinceId = aa.provinceId,
                    cityId = aa.cityId,
                    countyId = aa.countyId,
                    commissionRate = aa.commissionRate,
                    isEnable = aa.isEnable,
                    createTime = aa.createTime,
                    updateTime = aa.updateTime,
                    isDelete = aa.isDelete,
                    remark = aa.remark
                })
                .ToPageListAsync(page, limit, totalCount);

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = totalCount.Value;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetDetails
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<JsonResult> GetDetails()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id <= 0)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            var model = await _coreCmsAgentAreaServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id <= 0)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            var model = await _coreCmsAgentAreaServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> Edit([FromBody] CoreCmsAgentArea entity)
        {
            var jm = await _coreCmsAgentAreaServices.UpdateAsync(entity);
            return new JsonResult(jm);
        }



        // POST: Api/CoreCmsAgentArea/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsAgentArea entity)
        {
            var jm = await _coreCmsAgentAreaServices.InsertAsync(entity);
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("删除数据")]
        public async Task<JsonResult> Delete()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id <= 0)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            jm = await _coreCmsAgentAreaServices.DeleteByIdAsync(id);

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/DoDelete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("删除数据")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            if (entity?.id <= 0)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            jm = await _coreCmsAgentAreaServices.DeleteByIdAsync(entity.id);

            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/BatchDelete
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<JsonResult> BatchDelete()
        {
            var jm = new AdminUiCallBack();

            var ids = Request.Form["ids"].FirstOrDefault();
            if (string.IsNullOrEmpty(ids))
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            var idarr = ids.Split(",").Select(int.Parse).ToArray();
            jm = await _coreCmsAgentAreaServices.DeleteByIdsAsync(idarr);

            return new JsonResult(jm);
        }






        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取代理商列表")]
        public async Task<JsonResult> GetAgentList()
        {
            var jm = new AdminUiCallBack();
            var agents = await _coreCmsAgentServices.Queryable()
                .Where(p => p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.AgentVerifyStatus.VerifyYes)
                .OrderBy(p => p.id)
                .Select(p => new { value = p.id, label = p.name }).ToListAsync();

            if (agents == null || !agents.Any())
            {
                jm.code = 1;
                jm.msg = "无代理商数据";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = agents;
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetAreaTree
        /// <summary>
        /// 获取地区树形结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取地区树形结构")]
        public async Task<JsonResult> GetAreaTree()
        {
            var jm = new AdminUiCallBack();
            var areas = await _coreCmsAreaServices.GetTreeArea(null);
            jm.code = 0;
            jm.data = areas;
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/DoEdit
        /// <summary>
        /// 编辑提交（前端专用）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交（前端专用）")]
        public async Task<JsonResult> DoEdit()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            var commissionRate = Request.Form["commissionRate"].FirstOrDefault().ObjectToDecimal(0);
            var isEnable = Request.Form["isEnable"].FirstOrDefault().ObjectToBool();
            var remark = Request.Form["remark"].FirstOrDefault();

            if (id <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            var model = await _coreCmsAgentAreaServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "数据不存在";
                return new JsonResult(jm);
            }

            model.commissionRate = commissionRate;
            model.isEnable = isEnable;
            model.remark = remark;
            model.updateTime = DateTime.Now;

            jm = await _coreCmsAgentAreaServices.UpdateAsync(model);
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/DoSetIsEnable
        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("设置启用状态")]
        public async Task<JsonResult> DoSetIsEnable()
        {
            var jm = new AdminUiCallBack();

            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            var isEnable = Request.Form["isEnable"].FirstOrDefault().ObjectToBool();

            if (id <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            var model = await _coreCmsAgentAreaServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "数据不存在";
                return new JsonResult(jm);
            }

            model.isEnable = isEnable;
            model.updateTime = DateTime.Now;

            jm = await _coreCmsAgentAreaServices.UpdateAsync(model);
            if (jm.code == 0)
            {
                jm.msg = isEnable ? "启用成功" : "禁用成功";
            }
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetProvinceList
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取省份列表")]
        public async Task<JsonResult> GetProvinceList()
        {
            var jm = new AdminUiCallBack();
            var provinces = await _coreCmsAreaServices.QueryListByClauseAsync(p => p.depth == 1 && p.isShow == true);
            jm.code = 0;
            jm.data = provinces.Select(p => new { id = p.id, name = p.name }).ToList();
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetCityList
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取城市列表")]
        public async Task<JsonResult> GetCityList()
        {
            var jm = new AdminUiCallBack();
            var parentId = Request.Form["parentId"].FirstOrDefault().ObjectToInt(0);
            
            if (parentId <= 0)
            {
                jm.msg = "请选择省份";
                return new JsonResult(jm);
            }

            var cities = await _coreCmsAreaServices.QueryListByClauseAsync(p => p.parentId == parentId && p.depth == 2 && p.isShow == true);
            jm.code = 0;
            jm.data = cities.Select(p => new { id = p.id, name = p.name }).ToList();
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/GetCountyList
        /// <summary>
        /// 获取区县列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取区县列表")]
        public async Task<JsonResult> GetCountyList()
        {
            var jm = new AdminUiCallBack();
            var parentId = Request.Form["parentId"].FirstOrDefault().ObjectToInt(0);
            
            if (parentId <= 0)
            {
                jm.msg = "请选择城市";
                return new JsonResult(jm);
            }

            var counties = await _coreCmsAreaServices.QueryListByClauseAsync(p => p.parentId == parentId && p.depth == 3 && p.isShow == true);
            jm.code = 0;
            jm.data = counties.Select(p => new { id = p.id, name = p.name }).ToList();
            return new JsonResult(jm);
        }

        // POST: Api/CoreCmsAgentArea/DoSetCommission
        /// <summary>
        /// 设置佣金比例
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("设置佣金比例")]
        public async Task<JsonResult> DoSetCommission([FromBody] FMSetCommissionPost entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id <= 0)
            {
                jm.msg = "参数错误";
                return new JsonResult(jm);
            }

            if (entity.commissionRate < 0 || entity.commissionRate > 100)
            {
                jm.msg = "佣金比例必须在0-100之间";
                return new JsonResult(jm);
            }

            // 直接更新指定字段，避免查询操作
            var updateModel = new CoreCmsAgentArea
            {
                id = entity.id,
                commissionRate = entity.commissionRate,
                updateTime = DateTime.Now
            };

            var result = await _coreCmsAgentAreaServices.UpdateAsync(updateModel, new List<string> { "commissionRate", "updateTime" }, null, $"id = {entity.id}");
            if (result)
            {
                jm.code = 0;
                jm.msg = "佣金比例设置成功";
            }
            else
            {
                jm.msg = "更新失败";
            }
            return new JsonResult(jm);
        }
    }
}