using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Extensions;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using App.Ask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    public class AccountController : BaseController
    {
        private readonly PersonService personService;
        private readonly ConfigService siteConfigService;

        public AccountController(PersonService personService, ConfigService siteConfigService)
        {
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
            this.siteConfigService = siteConfigService ?? throw new ArgumentNullException(nameof(siteConfigService));
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("ContinueLogout");
            }
            else
            {
                AccountRegisterModel model = new AccountRegisterModel();
                RegisterConfig config = await this.siteConfigService.GetConfigAsync<RegisterConfig>();
                PageConfig pageConfig = await this.siteConfigService.GetConfigAsync<PageConfig>();
                model.IsEnableInviteCode = config.IsEnableInviteCode;
                model.AlertMsg = pageConfig.RegisterAlert;
                model.SiteName = (await this.siteConfigService.GetHomeConfigAsync()).SiteName;
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.Json(AjaxResult.CreateByMessage("您已经登录，请勿重复注册", false));
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string code = HttpContext.Session.GetVerifyCode();
                    if (code.ToLower() == model.Code.ToLower())
                    {
                        try
                        {
                            await personService.Register(model);
                            return this.Json(AjaxResult.CreateByMessage(""));
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("ContinueLogout");
            }
            else
            {
                PageConfig pageConfig = await this.siteConfigService.GetConfigAsync<PageConfig>();
                AccountLoginModel model = new AccountLoginModel();
                model.SiteName = (await this.siteConfigService.GetHomeConfigAsync()).SiteName;
                model.AlertMsg = pageConfig.LoginAlert;
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    model.ReturnUrl = returnUrl;
                }
                else
                {
                    model.ReturnUrl = "/";
                }
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.Json(AjaxResult.CreateByMessage("您已经登录，请勿重复登录", false));
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string code = HttpContext.Session.GetVerifyCode();
                    if (code.ToLower() == model.Code.ToLower())
                    {
                        try
                        {
                            await personService.Login(model);
                            return this.Json(AjaxResult.CreateByMessage(""));
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

        [Authorize]
        [HttpGet]
        public IActionResult ContinueLogout()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = "")
        {
            await this.HttpContext.LogoutAsync();
            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 拒绝访问
        /// </summary>
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult IsLogin()
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            else
            {
                return this.Json(AjaxResult.CreateByMessage("", false));
            }
        }
    }
}