using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 活动
    /// </summary>
    public class Activity : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 活动发起者
        /// </summary>
        [Index]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        [Index]
        public Guid PostId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        [Index]
        public ActivityType ActivityType { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Index]
        public DateTime DoTime { get; set; }
    }
}