using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 角色过滤选项
    /// </summary>
    public enum RoleFilterType
    {
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("全部")]
        All = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 1,

        /// <summary>
        /// 后台运营
        /// </summary>
        [Description("后台运营")]
        Master = 2,

        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 3
    }
}
