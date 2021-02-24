using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Models
{
    public class OpenGraphComponentModel
    {
        public string Url { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 是否是首页
        /// </summary>
        public bool IsHome { get; set; }
        /// <summary>
        /// 首页描叙
        /// </summary>
        public string HomeDescription { get; set; }
    }
}
