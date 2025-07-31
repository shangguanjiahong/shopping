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
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsAgentArea>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsAgentArea, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "agentId":
                    orderEx = p => p.agentId;
                    break;
                case "areaId":
                    orderEx = p => p.areaId;
                    break;
                case "areaDepth":
                    orderEx = p => p.areaDepth;
                    break;
                case "commissionRate":
                    orderEx = p => p.commissionRate;
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
            //地区ID int
            var areaId = Request.Form["areaId"].FirstOrDefault().ObjectToInt(0);
            if (areaId > 0)
            {
                where = where.And(p => p.areaId == areaId);
            }
            //地区深度 int
            var areaDepth = Request.Form["areaDepth"].FirstOrDefault().ObjectToInt(0);
            if (areaDepth > 0)
            {
                where = where.And(p => p.areaDepth == areaDepth);
            }
            //省级地区ID int
            var provinceId = Request.Form["provinceId"].FirstOrDefault().ObjectToInt(0);
            if (provinceId > 0)
            {
                where = where.And(p => p.provinceId == provinceId);
            }
            //市级地区ID int
            var cityId = Request.Form["cityId"].FirstOrDefault().ObjectToInt(0);
            if (cityId > 0)
            {
                where = where.And(p => p.cityId == cityId);
            }
            //县级地区ID int
            var countyId = Request.Form["countyId"].FirstOrDefault().ObjectToInt(0);
            if (countyId > 0)
            {
                where = where.And(p => p.countyId == countyId);
            }
            //佣金比例 decimal
            var commissionRate = Request.Form["commissionRate"].FirstOrDefault().ObjectToDecimal(0);
            if (commissionRate > 0)
            {
                where = where.And(p => p.commissionRate == commissionRate);
            }
            //创建时间 DateTime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }
            //更新时间 DateTime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }
            //是否删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDelete == true);
            }
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDelete == false);
            }
            //获取数据
            var list = await _coreCmsAgentAreaServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            
            // 获取代理商和地区信息
            var agentIds = list.Select(p => p.agentId).Distinct().ToList();
            var areaIds = list.Select(p => p.areaId).Distinct().ToList();
            
            var agents = await _coreCmsAgentServices.QueryListByClauseAsync(p => agentIds.Contains(p.id));
            var areas = await _coreCmsAreaServices.QueryListByClauseAsync(p => areaIds.Contains(p.id));
            
            // 组装返回数据
            var resultList = list.Select(item => new
            {
                id = item.id,
                agentId = item.agentId,
                agentName = agents.FirstOrDefault(a => a.id == item.agentId)?.name ?? "",
                areaId = item.areaId,
                areaName = areas.FirstOrDefault(a => a.id == item.areaId)?.name ?? "",
                areaDepth = item.areaDepth,
                provinceId = item.provinceId,
                cityId = item.cityId,
                countyId = item.countyId,
                commissionRate = item.commissionRate,
                isEnable = item.isEnable,
                createTime = item.createTime,
                updateTime = item.updateTime,
                isDelete = item.isDelete
            }).ToList();
            
            //返回数据
            jm.data = resultList;
            jm.code = 0;
            jm.count = list.TotalCount;
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

        // POST: Api/CoreCmsAgentArea/Add
        /// <summary>
        /// 新增提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("新增提交")]
        public async Task<JsonResult> Add([FromBody] CoreCmsAgentArea entity)
        {
            var jm = await _coreCmsAgentAreaServices.InsertAsync(entity);
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

        #region 首页数据============================================================
        // POST: Api/CoreCmsAgentArea/GetIndex
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

        // POST: Api/CoreCmsAgentArea/GetAgentList
        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取代理商列表")]
        public async Task<JsonResult> GetAgentList()
        {
            var jm = new AdminUiCallBack();
            var agents = await _coreCmsAgentServices.QueryListByClauseAsync(p => p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.AgentVerifyStatus.VerifyYes);
            jm.code = 0;
            jm.data = agents.Select(p => new { value = p.id, label = p.name }).ToList();
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

        // POST: Api/CoreCmsAgentArea/SetEnable
        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("设置启用状态")]
        public async Task<JsonResult> SetEnable()
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
        public async Task<JsonResult> DoSetCommission([FromBody] CoreCmsAgentArea entity)
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

            var model = await _coreCmsAgentAreaServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "数据不存在";
                return new JsonResult(jm);
            }

            model.commissionRate = entity.commissionRate;
            model.updateTime = DateTime.Now;

            jm = await _coreCmsAgentAreaServices.UpdateAsync(model);
            if (jm.code == 0)
            {
                jm.msg = "佣金比例设置成功";
            }
            return new JsonResult(jm);
        }
    }
}