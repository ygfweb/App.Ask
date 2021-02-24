using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    /// <summary>
    /// 点赞控制器
    /// </summary>
    public class LikeController : Controller
    {
        private readonly ZanService zanService;
        private readonly PostService postService;
        private readonly CommentService commentService;

        public LikeController(ZanService zanService, PostService postService, CommentService commentService)
        {
            this.zanService = zanService ?? throw new ArgumentNullException(nameof(zanService));
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
        }



        /// <summary>
        /// 对帖子点赞
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LikePost(Guid id)
        {
            try
            {
                await this.zanService.ZanPostAsync(id);
                Post post = await this.postService.GetPostByIdAsync(id);
                return Json(AjaxResult.CreateByContext(post.LikeNum));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }
        /// <summary>
        /// 对评论点赞
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LikeComment(Guid id)
        {
            try
            {
                await this.zanService.ZanCommentAsync(id);
                Comment comment = await this.commentService.GetByIdAsync(id);
                return Json(AjaxResult.CreateByContext(comment.LikeNum));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }
    }
}
