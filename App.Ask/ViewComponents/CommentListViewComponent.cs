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
    /// 评论列表
    /// </summary>
    public class CommentListViewComponent : ViewComponent
    {
        private readonly CommentService commentService;
        private readonly PersonService personService;

        public CommentListViewComponent(CommentService commentService, PersonService personService)
        {
            this.commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid pPostId)
        {
            List<CommentView> comments = await this.commentService.GetCommentViewsAsync(pPostId);
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            return View(new CommentListComponentModel() { CommentViews = comments, LoginUser = person, PostId = pPostId, CurrentUrl = this.HttpContext.Request.Path });
        }
    }
}
