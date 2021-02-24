using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class TopNavbarViewComponent : ViewComponent
    {
        private readonly DbFactory dbFactory;

        public TopNavbarViewComponent(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid? id = this.HttpContext.User.GetUserId();
                if (id == null)
                {
                    throw new Exception("用户已登录，但系统未在cookie中发现其ID");
                }
                using (var work = dbFactory.StartWork())
                {
                    Person person = await work.Person.SingleByIdAsync(id.Value);
                    string siteName = (await work.Config.GetHomeConfigAsync()).SiteName;
                    Role role = await work.Role.GetByIdAsync(person.RoleId);
                    return View(new TopNavbarComponentModel() { IsLogin = true, Person = person, SiteName = siteName, Role = role });
                }
            }
            else
            {
                using (var work = dbFactory.StartWork())
                {
                    string siteName = (await work.Config.GetHomeConfigAsync()).SiteName;
                    return View(new TopNavbarComponentModel() { IsLogin = false, Person = null, SiteName = siteName, Role = null });
                }
            }
        }
    }
}