using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Setting
{
    /// <summary>
    /// 页面配置
    /// </summary>
    public class PageConfig
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        [Display(Name = "每页行数")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 注册页面警告
        /// </summary>
        [Display(Name = "注册页面警告")]
        public string RegisterAlert { get; set; } = "";

        /// <summary>
        /// 登录页面警告
        /// </summary>
        [Display(Name = "登录页面警告")]
        public string LoginAlert { get; set; } = "";
    }
}