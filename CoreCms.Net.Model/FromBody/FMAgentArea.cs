using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    /// 设置代理商地区佣金比例提交参数
    /// </summary>
    public class FMSetCommissionPost
    {
        /// <summary>
        /// 代理商地区ID
        /// </summary>
        [Required(ErrorMessage = "请输入代理商地区ID")]
        public int id { get; set; }
        
        /// <summary>
        /// 佣金比例
        /// </summary>
        [Required(ErrorMessage = "请输入佣金比例")]
        [Range(0, 100, ErrorMessage = "佣金比例必须在0-100之间")]
        public decimal commissionRate { get; set; }
    }
}