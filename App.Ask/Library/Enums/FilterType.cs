using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 过滤类型
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// 所有最新
        /// </summary>
        [Description("最新")]
        New = 0,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Best = 1,
        /// <summary>
        /// 热门
        /// </summary>
        [Description("热门")]
        Popular,
        /// <summary>
        /// 待回
        /// </summary>
        [Description("待回")]
        ZeroResponse
    }
}
