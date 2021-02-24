using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 最新公告视图组件
    /// </summary>
    public class NewAnnounceViewComponent : ViewComponent
    {
        private readonly PostService postService;

        public NewAnnounceViewComponent(PostService postService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<PostView> posts = await this.postService.GetNewAnnounceAsync();
            NewAnnounceComponentModel model = new NewAnnounceComponentModel { postViews = posts };
            return View(model);
        }
    }
}
