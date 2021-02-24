using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Enums
{
    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("SuperAdmin")]
        SuperAdmin = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        [Description("Admin")]
        Admin = 1,

        /// <summary>
        /// 后台运营
        /// </summary>
        [Description("Master")]
        Master = 2,

        /// <summary>
        /// 用户
        /// </summary>
        [Description("User")]
        User = 3
    }
}