using System;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : BaseEntity
    {
        public Guid Id { get; set; }

        [Index]
        public string Name { get; set; }

        [Index]
        public RoleType RoleType { get; set; }
    }
}