using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 邀请码
    /// </summary>
    public class InviteCode : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 归属用户
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string Code { get; set; }
    }
}