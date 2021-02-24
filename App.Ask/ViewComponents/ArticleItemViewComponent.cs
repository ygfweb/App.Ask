using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using SiHan.Libs.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class ArticleItemViewComponent : ViewComponent
    {
        private readonly PostService postService;

        public ArticleItemViewComponent(PostService postService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IViewComponentResult> InvokeAsync(PostView forPostView)
        {
            if (forPostView == null)
            {
                throw new ArgumentNullException(nameof(forPostView));
            }
            Post post = ObjectMapper.Map<PostView, Post>(forPostView);
            PostData postData = await this.postService.GetPostDataAsync(post);
            ArticleItemComponentModel model = new ArticleItemComponentModel(forPostView, postData);
            return View(model);
        }
    }
}