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
    public class TopicController : Controller
    {
        private readonly TopicService topicService;
        private readonly PostService postService;

        public TopicController(TopicService topicService, PostService postService)
        {
            this.topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IActionResult> Details(Guid id, FilterType filter = FilterType.New, int page = 1)
        {
            Topic topic = await topicService.GetByIdAsync(id);
            if (topic == null || topic.IsHide)
            {
                return NotFound();
            }
            if (page <= 0)
            {
                return NotFound();
            }
            PagingResult<PostView> result = await this.postService.GetPagingResultAsync(id, PostResultType.All, page, filter);
            if (result.PageCount > 1 && page > result.PageCount)
            {
                return NotFound();
            }
            TopicDetailsModel model = new TopicDetailsModel
            {
                Topic = topic,
                PagingResult = result
            };
            return View(model);
        }
    }
}