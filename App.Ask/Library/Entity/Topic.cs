using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 话题
    /// </summary>
    public class Topic : BaseEntity
    {
        public Guid Id { get; set; }

        [Index]
        public string Name { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Index]
        public bool IsHide { get; set; }

        /// <summary>
        /// 是否属于公告（系统内只能有一个）
        /// </summary>
        public bool IsAnnounce { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }
    }
}
