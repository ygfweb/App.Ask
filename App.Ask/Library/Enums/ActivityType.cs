using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 活动类型（所有活动仅针对帖子）
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// 发布帖子
        /// </summary>
        Publish = 0,
        /// <summary>
        /// 编辑帖子
        /// </summary>
        Modify = 1,
        /// <summary>
        /// 发布评论
        /// </summary>
        Comment = 2,
        /// <summary>
        /// 收藏帖子
        /// </summary>
        Favorite = 3
    }
}