using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Info
{
    public class CommentInfo
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public Guid PostId { get; set; }
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string PostTitle;
        /// <summary>
        /// 评论ID
        /// </summary>
        public Guid CommentId { get; set; }
    }
}
