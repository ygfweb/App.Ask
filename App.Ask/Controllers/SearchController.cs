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
    public class SearchController : Controller
    {
        private readonly PostService postService;

        public SearchController(PostService postService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<IActionResult> Index(string q = "", int page = 1)
        {
            this.ViewData["IsHideSearch"] = true;
            PagingResult<PostView> pagingResult = await this.postService.SearchAsync(Guid.Empty, FilterType.New, q, page);
            SearchIndexModel model = new SearchIndexModel { q = q, PagingResult = pagingResult };
            return View(model);
        }
    }
}
