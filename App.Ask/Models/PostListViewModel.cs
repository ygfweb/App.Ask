using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    /// <summary>
    /// 帖子列表视图模型
    /// </summary>
    public class PostListViewModel
    {
        public List<PostView> PostViews { get; set; } = new List<PostView>();
    }
}
