using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
