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
    public class NewBestPostViewComponent : ViewComponent
    {
        private readonly PostService postService;

        public NewBestPostViewComponent(PostService postService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<PostView> posts = await this.postService.GetNewBestPostAsync();
            NewBestPostComponentModel model = new NewBestPostComponentModel { Posts = posts };
            return View(model);
        }
    }
}
