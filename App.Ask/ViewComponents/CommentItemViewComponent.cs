using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 评论元素视图组件
    /// </summary>
    public class CommentItemViewComponent : ViewComponent
    {
        private readonly DbFactory dbFactory;
        private readonly ZanService zanService;

        public CommentItemViewComponent(DbFactory dbFactory, ZanService zanService)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.zanService = zanService ?? throw new ArgumentNullException(nameof(zanService));
        }

        public IViewComponentResult Invoke(PersonView pLoginPerson, CommentView pCommentView)
        {
            bool isLike = zanService.IsCommentZanAsync(pCommentView.Id).Result;
            return View(new CommentItemComponentModel(pLoginPerson, pCommentView, this.dbFactory, isLike));
        }
    }
}