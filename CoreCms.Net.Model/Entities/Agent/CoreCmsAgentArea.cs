/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 代理商地区绑定表
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 代理商地区绑定表
    /// </summary>
    [SugarTable("CoreCmsAgentArea",TableDescription = "代理商地区绑定表")]
    public partial class CoreCmsAgentArea
    {
        /// <summary>
        /// 代理商地区绑定表
        /// </summary>
        public CoreCmsAgentArea()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        
        /// <summary>
        /// 代理商ID
        /// </summary>
        [Display(Name = "代理商ID")]
        [SugarColumn(ColumnDescription = "代理商ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 agentId { get; set; }
        
        /// <summary>
        /// 地区ID
        /// </summary>
        [Display(Name = "地区ID")]
        [SugarColumn(ColumnDescription = "地区ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 areaId { get; set; }
        
        /// <summary>
        /// 地区深度(1省2市3县)
        /// </summary>
        [Display(Name = "地区深度")]
        [SugarColumn(ColumnDescription = "地区深度(1省2市3县)")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 areaDepth { get; set; }
        
        /// <summary>
        /// 省级地区ID
        /// </summary>
        [Display(Name = "省级地区ID")]
        [SugarColumn(ColumnDescription = "省级地区ID", IsNullable = true)]
        public System.Int32? provinceId { get; set; }
        
        /// <summary>
        /// 市级地区ID
        /// </summary>
        [Display(Name = "市级地区ID")]
        [SugarColumn(ColumnDescription = "市级地区ID", IsNullable = true)]
        public System.Int32? cityId { get; set; }
        
        /// <summary>
        /// 县级地区ID
        /// </summary>
        [Display(Name = "县级地区ID")]
        [SugarColumn(ColumnDescription = "县级地区ID", IsNullable = true)]
        public System.Int32? countyId { get; set; }
        
        /// <summary>
        /// 佣金比例(百分比)
        /// </summary>
        [Display(Name = "佣金比例")]
        [SugarColumn(ColumnDescription = "佣金比例(百分比)")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal commissionRate { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 500)]
        public System.String remark { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [SugarColumn(ColumnDescription = "是否启用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isEnable { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDelete { get; set; }
        

    }
}