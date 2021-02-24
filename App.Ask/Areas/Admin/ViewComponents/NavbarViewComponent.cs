using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly DbFactory dbFactory;

        public NavbarViewComponent(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public IViewComponentResult Invoke()
        {
            this.ViewData["id"] = this.HttpContext.User.GetUserId();
            this.ViewData["NickName"] = this.HttpContext.User.GetUserName();
            return View();
        }
    }
}
