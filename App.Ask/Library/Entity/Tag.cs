using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Index]
        public string Name { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [Index]
        public bool IsBest { get; set; }

        /// <summary>
        /// 是否是系统创建，false表示由普通用户创建
        /// </summary>
        [Index]
        public bool IsSystem { get; set; }
    }
}