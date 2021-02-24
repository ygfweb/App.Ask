using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Setting;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 页脚版权组件
    /// </summary>
    public class CopyrightViewComponent : ViewComponent
    {
        private readonly ConfigService configService;

        public CopyrightViewComponent(ConfigService configService)
        {
            this.configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HomeConfig config = await this.configService.GetConfigAsync<HomeConfig>();
            this.ViewData["_SiteFooter"] = config.SiteFooter;
            return View();
        }
    }
}
