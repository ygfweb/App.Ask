using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using App.Ask.Library.Entity;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Models
{
    /// <summary>
    /// 话题导航组件模型
    /// </summary>
    public class TopicNavbarComponentModel
    {
        public List<Topic> Topics { get; set; } = new List<Topic>();
        /// <summary>
        /// 当前话题，为空表示所有
        /// </summary>
        public Topic Current { get; set; }

        private readonly HttpContext httpContext;

        public TopicNavbarComponentModel(List<Topic> topics, Topic current, HttpContext httpContext)
        {
            Topics = topics;
            Current = current;
            this.httpContext = httpContext;
        }

        public string GetUrl(Topic topic)
        {
            QueryWrapper queryWrapper = new QueryWrapper(this.httpContext);
            if (topic.Id == Guid.Empty)
            {
                return queryWrapper.RemoveValue("topic");
            }
            else
            {
                return queryWrapper.SetValue("topic", topic.Id.ToString());
            }
        }

        public string GetCss(Topic topic)
        {
            if (topic.Id == Current.Id)
            {
                return "nav-link active";
            }
            else
            {
                return "nav-link";
            }
        }
    }
}