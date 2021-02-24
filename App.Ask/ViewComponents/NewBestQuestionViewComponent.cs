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
    public class NewBestQuestionViewComponent : ViewComponent
    {
        private readonly PostService postService;

        public NewBestQuestionViewComponent(PostService postService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<PostView> posts = await this.postService.GetNewBestQuestionAsync();
            NewBestQuestionComponentModel model = new NewBestQuestionComponentModel { Posts = posts };
            return View(model);
        }
    }
}
