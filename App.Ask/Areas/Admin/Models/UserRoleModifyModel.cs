using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 用户角色修改模型
    /// </summary>
    public class UserRoleModifyModel
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public PersonView LoginUser { get; set; }
        /// <summary>
        /// 操作对象
        /// </summary>
        public PersonView DoUser { get; set; }

        public Guid Id { get; set; }
        public RoleType Role { get; set; }
    }
}
