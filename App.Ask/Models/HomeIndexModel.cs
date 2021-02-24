using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    /// <summary>
    /// 首页视图模型
    /// </summary>
    public class HomeIndexModel
    {
        public PagingResult<PostView> PostViews { get; set; }
        public string Search { get; set; } = string.Empty;
        public PersonView LoginPerson { get; set; }
    }
}