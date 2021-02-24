using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 收藏表
    /// </summary>
    public class Favorite : BaseEntity
    {
        public Guid Id { get; set; }
        [Index]
        public Guid PersonId { get; set; }
        [Index]
        public Guid PostId { get; set; }
        [Index]
        public DateTime DoTime { get; set; }
    }
}