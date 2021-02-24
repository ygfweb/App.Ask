using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 头衔
    /// </summary>
    public class Honor : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}