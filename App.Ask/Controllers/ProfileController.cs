using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Services;
using App.Ask.Library.Utils;
using App.Ask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    /// <summary>
    /// 账号设置控制器
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly PersonService personService;
        private readonly UploadService uploadService;

        public ProfileController(PersonService personService, UploadService uploadService)
        {
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
            this.uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        }



        /// <summary>
        /// 基本资料
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            ProfileInfoEditModel model = new ProfileInfoEditModel() { AccountName = person.AccountName, NickName = person.NickName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Info(ProfileInfoEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.personService.ModifyNickNameAsync(model);
                    return this.Json(AjaxResult.CreateDefaultSuccess());
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

        /// <summary>
        /// 修改头像
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ModifyAvatar()
        {
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            this.ViewData["avatar"] = person.Avatar;
            return View();
        }


        [HttpPost]
        [RequestSizeLimit(1024 * 1024 * 1)]
        public async Task<IActionResult> ModifyAvatar(IFormFile file)
        {
            try
            {
                PersonView person = await this.personService.GetCurrentPersonViewAsync();
                UploadInfo info = await this.uploadService.UploadImageAsync(file, true);
                await this.personService.ModifyAvatarAsync(person, info);
                return this.Json(AjaxResult.CreateByContext(info.UrlPath));
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpGet]
        public IActionResult ModifyPassword()
        {
            ProfileModifyPasswordModel model = new ProfileModifyPasswordModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyPassword(ProfileModifyPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string code = HttpContext.Session.GetVerifyCode();
                if (code.ToLower() == model.Code.ToLower())
                {
                    try
                    {
                        await personService.ModifyPasswordAsync(model);
                        return this.Json(AjaxResult.CreateDefaultSuccess());
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
                    await HttpContext.Session.RefreshVerifyCodeAsync();
                    AjaxResult result = new AjaxResult();
                    result.Success = false;
                    result.ErrorMessages.Add(nameof(model.Code), "验证码不正确，请重新输入");
                    return this.Json(result);
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }
    }
}


