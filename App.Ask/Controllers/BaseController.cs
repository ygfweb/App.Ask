using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Controllers
{
    public class BaseController : Controller
    {
        protected void SetTempMessage(string message)
        {
            this.TempData["_temp_msg"] = message;
        }
    }
}
