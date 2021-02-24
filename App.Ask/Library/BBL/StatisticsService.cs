using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Info;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 统计服务
    /// </summary>
    public class StatisticsService
    {
        private readonly DbFactory dbFactory;

        public StatisticsService(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// 获取注册用户总数
        /// </summary>
        public async Task<long> GetUserCount()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Person.CountAsync();
            }
        }

        /// <summary>
        /// 获取文章总数
        /// </summary>
        public async Task<long> GetArticleCountAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.GetArticleCountAsync();
            }
        }
        /// <summary>
        /// 获取问答总数
        /// </summary>
        public async Task<long> GetQuestionCountAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.GetQuestionCountAsync();
            }
        }

        /// <summary>
        /// 获取评论总数
        /// </summary>
        public async Task<long> GetCommentCountAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Comment.CountAsync();
            }
        }

        /// <summary>
        /// 获取最近一周的注册数量
        /// </summary>
        public async Task<List<PersonRegisterInfo>> GetWeekRegisterAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Person.GetWeekRegisterAsync();
            }
        }

    }
}
