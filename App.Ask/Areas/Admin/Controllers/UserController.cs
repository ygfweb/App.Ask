using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Utils.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : BaseController
    {
        private readonly PersonService personService;

        public UserController(PersonService personService)
        {
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int page = 1, string search = "", RoleFilterType roleFilter = RoleFilterType.All)
        {
            PagingResult<PersonView> result = await this.personService.SearchAsync(search, page, roleFilter, Library.Enums.SearchType.ALL);
            UserManageModel model = new UserManageModel(result, search, roleFilter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RoleModify(Guid id)
        {
            PersonView loginUser = await this.personService.GetCurrentPersonViewAsync();
            PersonView DoUser = await this.personService.GetPersonViewAsync(id);
            // 管理员不能修改其他管理员或站点管理员
            if (loginUser.RoleType == RoleType.Admin && (DoUser.RoleType == RoleType.Admin || DoUser.RoleType == RoleType.SuperAdmin))
            {
                return this.View("Forbid");
            }
            else
            {
                return View(new UserRoleModifyModel { DoUser = DoUser, LoginUser = loginUser, Id = DoUser.Id, Role = DoUser.RoleType });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RoleModify(Guid id, RoleType role)
        {
            try
            {
                PersonView loginUser = await this.personService.GetCurrentPersonViewAsync();
                PersonView DoUser = await this.personService.GetPersonViewAsync(id);
                if (DoUser == null)
                {
                    throw new Exception("该用户不存在");
                }
                else if (DoUser.RoleType == RoleType.SuperAdmin)
                {
                    throw new Exception("无权修改网站管理员");
                }
                else if (role == RoleType.SuperAdmin)
                {
                    throw new Exception("无权设置为网站管理员");
                }
                else if (loginUser.RoleType != RoleType.Admin && loginUser.RoleType != RoleType.SuperAdmin)
                {
                    throw new Exception("仅管理员拥有操作权限");
                }
                else if (loginUser.RoleType == RoleType.Admin && (role == RoleType.Admin || role == RoleType.SuperAdmin))
                {
                    throw new Exception("该操作无权限");
                }
                else
                {
                    await this.personService.ModifyRoleAsync(DoUser.Id, role);
                    return this.Json(AjaxResult.CreateDefaultSuccess());
                }
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                await this.personService.RemoveAsync(id);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 恢复被删除的用户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RestoreRemove(Guid id)
        {
            try
            {
                await this.personService.RestoreRemoveAsync(id);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 禁言用户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Mute(Guid id)
        {
            try
            {
                await this.personService.MuteByIdAsync(id);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        /// <summary>
        /// 恢复被禁言的用户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RestoreMute(Guid id)
        {
            try
            {
                await this.personService.RestoreMuteByIdAsync(id);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }
    }
}

