using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class BootstrapPagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int forPageSize, long forRowCount)
        {
            BootstrapPagerModel model = new BootstrapPagerModel(forPageSize, forRowCount, this.HttpContext);
            return View(model);
        }
    }
}