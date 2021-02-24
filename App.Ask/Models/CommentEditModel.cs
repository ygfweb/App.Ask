using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;

namespace App.Ask.Models
{
    /// <summary>
    /// 评论编辑模型
    /// </summary>
    public class CommentEditModel
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 父评论ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 帖子类型
        /// </summary>
        public PostType PostType { get; set; }

    }
}

