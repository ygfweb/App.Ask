using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Info
{
    /// <summary>
    /// 用户注册统计信息
    /// </summary>
    public class PersonRegisterInfo : BaseEntity
    {
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 注册数量
        /// </summary>
        public long CreateCount { get; set; }
    }
}


