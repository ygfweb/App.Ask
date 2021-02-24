using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 帖子状态
    /// </summary>
    public enum PostStatus
    {
        /// <summary>
        /// 屏蔽
        /// </summary>
        Block = 0,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hide = 1,
        /// <summary>
        /// 等待审核
        /// </summary>
        Verify = 2,
        /// <summary>
        /// 已发布
        /// </summary>
        Publish = 3
    }
}
