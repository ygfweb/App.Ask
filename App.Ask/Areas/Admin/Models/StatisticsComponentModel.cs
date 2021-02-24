using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 统计组件模型
    /// </summary>
    public class StatisticsComponentModel
    {
        /// <summary>
        /// 注册用户总数
        /// </summary>
        public long UserCount { get; set; }
        /// <summary>
        /// 文章总数
        /// </summary>
        public long ArticleCount { get; set; }
        /// <summary>
        /// 提问总数
        /// </summary>
        public long QuestionCount { get; set; }
        /// <summary>
        /// 评论总数
        /// </summary>
        public long CommentCount { get; set; }
    }
}