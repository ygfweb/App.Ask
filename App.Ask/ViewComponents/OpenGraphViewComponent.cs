using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Setting;
using App.Ask.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// OG协议视图组件
    /// </summary>
    public class OpenGraphViewComponent : ViewComponent
    {
        private readonly ConfigService configService;

        public OpenGraphViewComponent(ConfigService configService)
        {
            this.configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HomeConfig config = await this.configService.GetHomeConfigAsync();
            string url = this.HttpContext.Request.GetEncodedUrl();
            OpenGraphComponentModel model = new OpenGraphComponentModel()
            {
                SiteName = config.SiteName,
                Url = url,
                IsHome = this.HttpContext.Request.Path == "/",
                HomeDescription = config.SeoDescription
            };
            return View(model);
        }
    }
}


