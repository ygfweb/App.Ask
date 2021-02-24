using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 帖子基本信息
    /// </summary>
    public class PostBaseInfo : BaseEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 所属话题
        /// </summary>
        public Guid TopicId { get; set; }

        /// <summary>
        /// 帖子类型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否精华帖
        /// </summary>
        public bool IsBest { get; set; }

        /// <summary>
        /// 帖子状态
        /// </summary>
        public PostStatus PostStatus { get; set; }

        /// <summary>
        /// 评论总数
        /// </summary>
        public int CommentNum { get; set; }

        /// <summary>
        /// 点赞总数
        /// </summary>
        public int LikeNum { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        public int CollectNum { get; set; }
    }
}
