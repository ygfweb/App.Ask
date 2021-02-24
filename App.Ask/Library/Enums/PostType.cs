using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 帖子类型
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// 问答
        /// </summary>
        [Description("问题")]
        Question = 0,
        /// <summary>
        /// 文章
        /// </summary>
        [Description("文章")]
        Article = 1
    }
}
