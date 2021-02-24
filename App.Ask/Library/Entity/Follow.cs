using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 关注表
    /// </summary>
    public class Follow : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 关注者（即粉丝）
        /// </summary>
        [Index]
        public Guid FromPersonId { get; set; }

        /// <summary>
        /// 被关注者
        /// </summary>
        [Index]
        public Guid ToPersonId { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime DoTime { get; set; }
    }
}