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
    /// 推荐标签
    /// </summary>
    public class BestTagViewComponent : ViewComponent
    {
        private readonly TagService tagService;

        public BestTagViewComponent(TagService tagService)
        {
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Tag> tags = await this.tagService.GetBestTagsAsync();
            BestTagComponentModel model = new BestTagComponentModel
            {
                Tags = tags
            };
            return View(model);
        }
    }
}
