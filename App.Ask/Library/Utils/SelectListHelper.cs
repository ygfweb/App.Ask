using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Ask.Library.Utils
{
    public class SelectListHelper
    {
        /// <summary>
        /// 将话题转换为选择列表
        /// </summary>
        public static List<SelectListItem> FromTopic(List<Topic> topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem("请选择...", ""));
            foreach (var topic in topics)
            {
                items.Add(new SelectListItem(topic.Name, topic.Id.ToString()));
            }
            return items;
        }
    }
}
