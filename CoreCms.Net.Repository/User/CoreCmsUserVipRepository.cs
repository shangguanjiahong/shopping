/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: VIP会员数据访问层实现
***********************************************************************/ 
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// VIP会员数据访问层实现
    /// </summary>
    public class CoreCmsUserVipRepository : BaseRepository<CoreCmsUserVip>, ICoreCmsUserVipRepository
    {
        public CoreCmsUserVipRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}