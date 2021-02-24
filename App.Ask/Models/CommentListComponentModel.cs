using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Models
{
    /// <summary>
    /// 评论列表模型
    /// </summary>
    public class CommentListComponentModel
    {
        /// <summary>
        /// 评论列表
        /// </summary>
        public List<CommentView> CommentViews { get; set; } = new List<CommentView>();
        /// <summary>
        /// 登录用户
        /// </summary>
        public PersonView LoginUser { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public Guid PostId { get; set; }

        public string CurrentUrl { get; set; }
    }
}