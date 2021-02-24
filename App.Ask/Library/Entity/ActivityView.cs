using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 活动视图
    /// </summary>
    public class ActivityView : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 活动发起者
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityType ActivityType { get; set; }

        /// <summary>
        /// 活动时间
        /// </summary>
        public DateTime DoTime { get; set; }
        /// <summary>
        /// 发起者账号
        /// </summary>
        public string PersonAccountName { get; set; }
        /// <summary>
        /// 发起者昵称
        /// </summary>
        public string PersonNickName { get; set; }
        /// <summary>
        /// 发起者头像
        /// </summary>
        public string PersonAvatar { get; set; }
        /// <summary>
        /// 发起者是否被删除
        /// </summary>
        public bool PersonIsDelete { get; set; }
        /// <summary>
        /// 发起者是否被禁言
        /// </summary>
        public bool PersonIsMute { get; set; }
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string PostTitle { get; set; }
        /// <summary>
        /// 帖子类型
        /// </summary>
        public PostType PostType { get; set; }
        /// <summary>
        /// 帖子状态
        /// </summary>
        public PostStatus PostStatus { get; set; }
        /// <summary>
        /// 帖子所属话题
        /// </summary>
        public Guid TopicId { get; set; }
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
