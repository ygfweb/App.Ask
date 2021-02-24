using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 用户头衔
    /// </summary>
    public class UserHonor : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Index]
        public Guid UserId { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        [Index]
        public Guid HonorId { get; set; }
    }
}
