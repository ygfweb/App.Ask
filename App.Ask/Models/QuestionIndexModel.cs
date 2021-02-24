using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    /// <summary>
    /// 问答首页模型
    /// </summary>
    public class QuestionIndexModel
    {
        public PagingResult<PostView> PostViews { get; set; }
    }
}
