using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 个人资料修改侧边栏
    /// </summary>
    public class ProfileSidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
