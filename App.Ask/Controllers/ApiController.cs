using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    public class ApiController : Controller
    {
        private readonly PostService postService;
        private readonly PersonService personService;

        public ApiController(PostService postService, PersonService personService)
        {
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }


        /// <summary>
        /// 设为精华帖
        /// </summary>
        [Authorize(Roles = "SuperAdmin,Admin,Master")]
        [HttpPost]
        public async Task<IActionResult> SetBest(Guid postId, bool isBest)
        {
            try
            {
                await this.postService.SetBestAsync(postId, isBest);
                return Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 设为置顶帖
        /// </summary>
        [Authorize(Roles = "SuperAdmin,Admin,Master")]
        [HttpPost]
        public async Task<IActionResult> SetTop(Guid postId, bool isTop)
        {
            try
            {
                await this.postService.SetTopAsync(postId, isTop);
                return Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 屏蔽帖子
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BlockPost(Guid postId)
        {
            try
            {
                PersonView loginPerson = await this.personService.GetCurrentPersonViewAsync();
                Post post = await this.postService.GetPostByIdAsync(postId);
                if (post == null)
                {
                    throw new Exception("该帖子不存在");
                }
                bool isSelf = loginPerson.Id == post.PersonId;
                bool isAdmin = loginPerson.RoleType == RoleType.Admin || loginPerson.RoleType == RoleType.Master || loginPerson.RoleType == RoleType.SuperAdmin;
                if (isSelf || isAdmin)
                {
                    await this.postService.BlockAsync(postId, loginPerson);
                    return Json(AjaxResult.CreateDefaultSuccess());
                }
                else
                {
                    throw new Exception("没有该操作的权限");
                }
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

    }
}
