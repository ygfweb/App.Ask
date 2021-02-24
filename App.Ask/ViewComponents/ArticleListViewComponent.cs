using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class ArticleListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<PostView> forPostViews)
        {
            if (forPostViews == null)
            {
                throw new ArgumentNullException(nameof(forPostViews));
            }
            ArticleListComponentModel model = new ArticleListComponentModel(forPostViews);
            return View(model);
        }
    }
}
