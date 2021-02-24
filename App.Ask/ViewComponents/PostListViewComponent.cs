using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using SiHan.Libs.Utils.Paging;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 帖子列表视图
    /// </summary>
    public class PostListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<PostView> pPostViews)
        {
            PostListViewModel model = new PostListViewModel
            {
                PostViews = pPostViews
            };
            return View(model);
        }
    }
}