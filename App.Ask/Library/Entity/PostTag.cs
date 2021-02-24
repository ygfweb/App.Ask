using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 帖子标签关联表
    /// </summary>
    public class PostTag : BaseEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 帖子ID
        /// </summary>
        [Index]
        public Guid PostId { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        [Index]
        public Guid TagId { get; set; }
    }
}
