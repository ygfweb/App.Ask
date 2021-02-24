using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Utils.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 过滤导航组件
    /// </summary>
    public class FilterNavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            FilterNavbarComponentModel model = new FilterNavbarComponentModel(this.HttpContext);
            return View(model);
        }
    }
}
