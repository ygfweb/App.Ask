using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Models
{
    /// <summary>
    /// 文章列表组件
    /// </summary>
    public class ArticleListComponentModel
    {
        public List<PostView> PostViews { get; set; } = new List<PostView>();

        public ArticleListComponentModel(List<PostView> postViews)
        {
            PostViews = postViews ?? throw new ArgumentNullException(nameof(postViews));
        }
    }
}
