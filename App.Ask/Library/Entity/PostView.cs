using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 帖子视图
    /// </summary>
    public class PostView : BaseEntity
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

        /// <summary>
        /// 浏览数
        /// </summary>
        public int ViewNum { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string PersonNickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string PersonAvatar { get; set; }

        /// <summary>
        /// 用户是否已被删除
        /// </summary>
        public bool PersonIsDelete { get; set; }
        /// <summary>
        /// 用户是否已被禁言
        /// </summary>
        public bool PersonIsMute { get; set; }
        /// <summary>
        /// 话题名称
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// 话题是否被隐藏
        /// </summary>
        public bool TopicIsHide { get; set; }
        /// <summary>
        /// 话题是否属于公告
        /// </summary>
        public bool TopicIsAnnounce { get; set; }
    }
}
