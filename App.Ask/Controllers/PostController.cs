using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Extensions;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly PostService postService;
        private readonly PersonService personService;

        public PostController(PostService postService, PersonService personService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsk()
        {
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            PostEditModel model = await postService.CreateNewEditModelAsync(PostType.Question, person);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsk(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Post post = await this.postService.InsertAsync(model, this.User.GetUserId().Value);
                    AjaxResult result = new AjaxResult
                    {
                        Message = "提交成功",
                        Context = Url.Action("Details", "Question", new { id = post.Id })
                    };
                    return this.Json(result);
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModifyAsk(Guid id)
        {
            Post post = await this.postService.GetPostByIdAsync(id);
            if (post == null || post.PostStatus == PostStatus.Block || post.PostType != PostType.Question)
            {
                return NotFound();
            }
            else if (post.PersonId != this.User.GetUserId())
            {
                return this.Forbid();
            }
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            PostEditModel model = await postService.CreateNewEditModelAsync(post, person);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ModifyAsk(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.postService.ModifyAsync(model, this.User.GetUserId().Value);
                    AjaxResult result = new AjaxResult
                    {
                        Message = "提交成功",
                        Context = Url.Action("Details", "Question", new { id = model.Id })
                    };
                    return this.Json(result);
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateArticle()
        {
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            PostEditModel model = await postService.CreateNewEditModelAsync(PostType.Article, person);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Post post = await this.postService.InsertAsync(model, this.User.GetUserId().Value);
                    AjaxResult result = new AjaxResult
                    {
                        Message = "提交成功",
                        Context = Url.Action("Details", "Article", new { id = post.Id })
                    };
                    return this.Json(result);
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModifyArticle(Guid id)
        {
            Post post = await this.postService.GetPostByIdAsync(id);
            if (post == null || post.PostStatus == PostStatus.Block)
            {
                return NotFound();
            }
            else if (post.PersonId != this.User.GetUserId())
            {
                return this.Forbid();
            }
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            PostEditModel model = await postService.CreateNewEditModelAsync(post, person);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyArticle(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.postService.ModifyAsync(model, this.User.GetUserId().Value);
                    AjaxResult result = new AjaxResult
                    {
                        Message = "提交成功",
                        Context = Url.Action("Details", "Article", new { id = model.Id })
                    };
                    return this.Json(result);
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }
    }
}



