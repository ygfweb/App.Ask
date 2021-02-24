using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    public class QuestionItemViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PostView forPostView)
        {
            if (forPostView == null)
            {
                throw new ArgumentNullException(nameof(forPostView));
            }
            QuestionItemComponentModel model = new QuestionItemComponentModel(forPostView);
            return View(model);
        }
    }
}
