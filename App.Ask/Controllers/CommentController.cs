using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Services;
using App.Ask.Library.Utils;
using App.Ask.Models;
using App.Ask.ViewComponents;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    /// <summary>
    /// 评论控制器
    /// </summary>
    [Authorize]
    public class CommentController : Controller
    {

        private readonly CommentService commentService;
        private readonly PersonService personService;
        private readonly HtmlService htmlService;
        private readonly PostService postService;

        public CommentController(CommentService commentService, PersonService personService, HtmlService htmlService, PostService postService)
        {
            this.commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
            this.htmlService = htmlService ?? throw new ArgumentNullException(nameof(htmlService));
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentEditModel model)
        {
            CommentView comment = await this.commentService.InsertAsync(model, this.User.GetUserId().Value);
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            return ViewComponent(typeof(CommentItemViewComponent), new { pLoginPerson = person, pCommentView = comment });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await this.commentService.DeleteAsync(id, this.User.GetUserId().Value);
                return Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Guid? loginUserId = this.User.GetUserId();
            Comment comment = await this.commentService.GetByIdAsync(id);
            if (comment == null || comment.IsDelete)
            {
                return NotFound();
            }
            else if (loginUserId == null || loginUserId.Value != comment.PersonId)
            {
                return this.Forbid();
            }
            else
            {

                CommentEditModel model = new CommentEditModel
                {
                    Id = comment.Id,
                    Content = comment.HtmlContent,
                    ParentId = comment.ParentId,
                    PostId = comment.PostId,
                    PostType = (await this.postService.GetPostTypeAsync(comment.PostId)).Value
                };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == null)
                    {
                        throw new Exception("评论ID不能为空");
                    }
                    Guid? loginUserId = this.User.GetUserId();
                    Comment comment = await this.commentService.GetByIdAsync(model.Id.Value);
                    if (comment == null || comment.IsDelete)
                    {
                        throw new Exception("该评论不存在，或已被删除");
                    }
                    else if (loginUserId == null || loginUserId.Value != comment.PersonId)
                    {
                        throw new Exception("没有该操作的权限");
                    }
                    else if (string.IsNullOrWhiteSpace(model.Content))
                    {
                        throw new Exception("内容不能为空");
                    }
                    else
                    {
                        string htmlContent = this.htmlService.ClearHtml(model.Content);
                        string textContent = this.htmlService.HtmlToText(htmlContent);
                        if (string.IsNullOrWhiteSpace(htmlContent))
                        {
                            throw new Exception("内容不能为空");
                        }
                        else
                        {
                            await this.commentService.ModifyAsync(comment.Id, htmlContent, textContent);
                            return Json(AjaxResult.CreateDefaultSuccess());
                        }
                    }
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
