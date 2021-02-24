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
    /// 活动列表视图组件
    /// </summary>
    public class ActivityListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<ActivityView> pActivityViews)
        {
            ActivityListComponentModel model = new ActivityListComponentModel
            {
                ActivityViews = pActivityViews
            };
            return View(model);
        }
    }
}