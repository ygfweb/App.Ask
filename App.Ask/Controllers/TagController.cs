using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Models;
using SiHan.Libs.Utils.Paging;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    public class TagController : Controller
    {
        private readonly PostService postService;
        private readonly TagService tagService;

        public TagController(PostService postService, TagService tagService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public async Task<IActionResult> Details(Guid id, FilterType filter = FilterType.New, int page = 1)
        {
            Tag tag = await tagService.SingleByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            if (page <= 0)
            {
                return NotFound();
            }
            PagingResult<PostView> result = await this.postService.GetPagingResultAsync(tag, page, filter);
            if (result.PageCount > 1 && page > result.PageCount)
            {
                return NotFound();
            }
            TagDetailsModel model = new TagDetailsModel
            {
                Tag = tag,
                PagingResult = result
            };
            return View(model);
        }
    }
}
