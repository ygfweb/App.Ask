using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string pTitle)
        {
            this.ViewData["_banner_title"] = pTitle;
            return View();
        }
    }
}
