using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 帖子项目
    /// </summary>
    public class PostItemViewComponent : ViewComponent
    {
        private readonly TagService tagService;
        public PostItemViewComponent(TagService tagService)
        {
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public async Task<IViewComponentResult> InvokeAsync(PostView post)
        {
            List<Tag> tags = await this.tagService.GetTagsByPost(post);
            PostItemViewModel model = new PostItemViewModel(post, tags);
            return View(model);
        }
    }
}

