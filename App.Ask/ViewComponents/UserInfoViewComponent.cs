using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 用户信息组件
    /// </summary>
    public class UserInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PersonView forPersonView)
        {
            UserInfoModel model = new UserInfoModel() { PersonView = forPersonView };
            return View(model);
        }
    }
}
