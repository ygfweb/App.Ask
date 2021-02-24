using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 帖子
    /// </summary>
    public class Post : BaseEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Index]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        [Index]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 所属话题
        /// </summary>
        [Index]
        public Guid TopicId { get; set; }

        /// <summary>
        /// 帖子类型
        /// </summary>
        [Index]
        public PostType PostType { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [Index]
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否精华帖
        /// </summary>
        [Index]
        public bool IsBest { get; set; }

        /// <summary>
        /// 帖子状态
        /// </summary>
        [Index]
        public PostStatus PostStatus { get; set; }

        /// <summary>
        /// 评论总数
        /// </summary>
        [Index]
        public int CommentNum { get; set; }

        /// <summary>
        /// 点赞总数
        /// </summary>
        [Index]
        public int LikeNum { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        [Index]
        public int CollectNum { get; set; }

        /// <summary>
        /// 浏览数
        /// </summary>
        [Index]
        public int ViewNum { get; set; }
    }
}