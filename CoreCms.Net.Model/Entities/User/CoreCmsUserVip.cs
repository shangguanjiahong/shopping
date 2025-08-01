/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: VIP会员实体类
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// VIP会员表
    /// </summary>
    [SugarTable("CoreCmsUserVip", "VIP会员表")]
    public partial class CoreCmsUserVip
    {
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }

        /// <summary>
        /// VIP等级
        /// </summary>
        [Display(Name = "VIP等级")]
        [SugarColumn(ColumnDescription = "VIP等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 vipLevel { get; set; }

        /// <summary>
        /// VIP开始时间
        /// </summary>
        [Display(Name = "VIP开始时间")]
        [SugarColumn(ColumnDescription = "VIP开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime startTime { get; set; }

        /// <summary>
        /// VIP结束时间
        /// </summary>
        [Display(Name = "VIP结束时间")]
        [SugarColumn(ColumnDescription = "VIP结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime endTime { get; set; }

        /// <summary>
        /// VIP状态 1=有效 2=过期
        /// </summary>
        [Display(Name = "VIP状态")]
        [SugarColumn(ColumnDescription = "VIP状态 1=有效 2=过期")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Display(Name = "支付金额")]
        [SugarColumn(ColumnDescription = "支付金额", Length = 10, DecimalDigits = 2)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal payAmount { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        [Display(Name = "支付订单号")]
        [SugarColumn(ColumnDescription = "支付订单号", Length = 50)]
        public System.String payOrderId { get; set; }

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
        [SugarColumn(ColumnDescription = "更新时间")]
        public System.DateTime? updateTime { get; set; }
    }
}