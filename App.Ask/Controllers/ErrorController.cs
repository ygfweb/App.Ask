using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Utils;
using SiHan.Html.Templates;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404)
                {
                    return this.View("404");
                }
                else if (statusCode.Value == 500)
                {
                    return new HtmlResult(HtmlTemplateProvider.Error500, 500);
                }
                else if (statusCode.Value == 503)
                {
                    return new HtmlResult(HtmlTemplateProvider.Error503, 503);
                }
                else
                {
                    return new HtmlResult(HtmlTemplateProvider.ErrorDefault, 500);
                }
            }
            else
            {
                return new HtmlResult(HtmlTemplateProvider.ErrorDefault, 500);
            }
        }
    }
}
