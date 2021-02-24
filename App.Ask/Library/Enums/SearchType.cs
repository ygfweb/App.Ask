using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 查询类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// 所有
        /// </summary>
        ALL,
        /// <summary>
        /// 仅查询可见
        /// </summary>
        Visible,
        /// <summary>
        /// 仅查询隐藏
        /// </summary>
        Hide
    }
}
