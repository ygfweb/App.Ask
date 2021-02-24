using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    /// <summary>
    /// 文章首页模型
    /// </summary>
    public class ArticleIndexModel
    {
        public PagingResult<PostView> PostViews { get; set; }
    }
}


