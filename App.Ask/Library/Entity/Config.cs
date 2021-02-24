using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 网站配置
    /// </summary>
    public class Config : BaseEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string ConfigKey { get; set; }
        /// <summary>
        /// 值（一般存储JSON格式）
        /// </summary>
        public string ConfigValue { get; set; }
    }
}

