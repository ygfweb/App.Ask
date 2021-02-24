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
    public class QuestionController : Controller
    {
        private readonly PostService postService;
        private readonly PersonService personService;

        public QuestionController(PostService postService, PersonService personService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        public async Task<IActionResult> Index(Guid topic, FilterType filter = FilterType.New, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PagingResult<PostView> postViews = await postService.GetPagingResultAsync(topic, PostResultType.Question, page, filter);
            if (postViews.PageCount > 1 && page > postViews.PageCount)
            {
                return NotFound();
            }
            QuestionIndexModel model = new QuestionIndexModel
            {
                PostViews = postViews
            };
            return this.View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Post post = await this.postService.GetPostByIdAsync(id);
            if (post == null || post.PostStatus != PostStatus.Publish || post.PostType != PostType.Question)
            {
                return NotFound();
            }
            else
            {
                await this.postService.IncreaseViewNumAsync(post);
                PersonView person = await this.personService.GetPersonViewAsync(post.PersonId);
                ArticleDetailsModel model = new ArticleDetailsModel { Post = post, Person = person };
                return View(model);
            }
        }
    }
}