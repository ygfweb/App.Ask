using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Models
{
    /// <summary>
    /// 最新公告组件模型
    /// </summary>
    public class NewAnnounceComponentModel
    {
        public List<PostView> postViews { get; set; } = new List<PostView>();
    }
}
