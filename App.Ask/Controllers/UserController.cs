using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using SiHan.Libs.Mapper;
using SiHan.Libs.Utils.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly PersonService personService;
        private readonly ActivityService activityService;
        private readonly PostService postService;
        private readonly FavoriteService favoriteService;

        public UserController(PersonService personService, ActivityService activityService, PostService postService, FavoriteService favoriteService)
        {
            this.personService = personService;
            this.activityService = activityService;
            this.postService = postService;
            this.favoriteService = favoriteService;
        }

        /// <summary>
        /// 个人主页
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Page(Guid id, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PersonView person = await this.personService.GetPersonViewAsync(id);
            if (person == null || person.IsDelete)
            {
                return NotFound();
            }
            else
            {
                Person p = ObjectMapper.Map<PersonView, Person>(person);
                PagingResult<ActivityView> pagingResult = await this.activityService.GetPagingResultsAsync(p, page);
                UserPageViewModel model = new UserPageViewModel(person, pagingResult);
                return View(model);
            }
        }

        /// <summary>
        /// 个人文章
        /// </summary>
        /// <param name="id">用户ID（不是文章ID）</param>
        public async Task<IActionResult> Article(Guid id, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PersonView person = await this.personService.GetPersonViewAsync(id);
            if (person == null || person.IsDelete)
            {
                return NotFound();
            }
            else
            {
                Person p = ObjectMapper.Map<PersonView, Person>(person);
                PagingResult<PostView> pagingResult = await this.postService.GetPagingResultAsync(person, Library.Enums.PostResultType.Article, page);
                if (pagingResult.PageCount > 1 && page > pagingResult.PageCount)
                {
                    return NotFound();
                }
                UserArticleViewModel model = new UserArticleViewModel(pagingResult, person);
                return View(model);
            }
        }
        /// <summary>
        /// 个人提问
        /// </summary>
        public async Task<IActionResult> Question(Guid id, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PersonView person = await this.personService.GetPersonViewAsync(id);
            if (person == null || person.IsDelete)
            {
                return NotFound();
            }
            else
            {
                Person p = ObjectMapper.Map<PersonView, Person>(person);
                PagingResult<PostView> pagingResult = await this.postService.GetPagingResultAsync(person, Library.Enums.PostResultType.Question, page);
                if (pagingResult.PageCount > 1 && page > pagingResult.PageCount)
                {
                    return NotFound();
                }
                UserQuestionViewModel model = new UserQuestionViewModel(pagingResult, person);
                return View(model);
            }
        }

        /// <summary>
        /// 个人收藏
        /// </summary>
        public async Task<IActionResult> Favorite(Guid id, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PersonView person = await this.personService.GetPersonViewAsync(id);
            if (person == null || person.IsDelete)
            {
                return NotFound();
            }
            else
            {
                PagingResult<PostView> pagingResult = await this.favoriteService.GetFavoritePagingResultAsync(id, page);
                if (pagingResult.PageCount > 1 && page > pagingResult.PageCount)
                {
                    return NotFound();
                }
                UserFavoriteViewModel model = new UserFavoriteViewModel(pagingResult, person);
                return View(model);
            }
        }
    }
}
