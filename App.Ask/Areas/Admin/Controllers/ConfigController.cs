using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ConfigController : BaseController
    {
        private readonly ConfigService configService;

        public ConfigController(ConfigService configService)
        {
            this.configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        [HttpGet]
        public async Task<IActionResult> HomeConfig()
        {
            HomeConfig homeConfig = await configService.GetHomeConfigAsync();
            return View(homeConfig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeConfig(HomeConfig model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    await this.configService.SetHomeConfigAsync(model);
                    this.SetTempMessage("操作成功");
                    return this.RedirectToAction("HomeConfig", "Config", new { Area = "Admin" });
                }
                catch (Exception e)
                {
                    this.SetTempMessage(e.Message);
                    return this.View(model);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> PageConfig()
        {
            PageConfig config = await configService.GetConfigAsync<PageConfig>();
            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PageConfig(PageConfig model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    await this.configService.SetConfigAsync(model);
                    this.SetTempMessage("操作成功");
                    return this.RedirectToAction(nameof(PageConfig), "Config", new { Area = "Admin" });
                }
                catch (Exception e)
                {
                    this.SetTempMessage(e.Message);
                    return this.View(model);
                }
            }
        }
    }
}

