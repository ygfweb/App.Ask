using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Setting;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 活动服务
    /// </summary>
    public class ActivityService
    {
        private readonly DbFactory dbFactory;

        public ActivityService(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// 获取指定用户的活动
        /// </summary>
        public async Task<PagingResult<ActivityView>> GetPagingResultsAsync(Person person, int page)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PageConfig pageConfig = await work.Config.GetConfigAsync<PageConfig>();
                return await work.ActivityView.GetPagingResultAsync(person, page, pageConfig.PageSize);
            }
        }
    }
}
