using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Time;

namespace App.Ask.Models
{
    public class PostItemViewModel
    {
        public PostView PostView { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public PostItemViewModel(PostView postView, List<Tag> tags)
        {
            PostView = postView ?? throw new ArgumentNullException(nameof(postView));
            this.Tags = tags;
        }

        public string GetPublishTime()
        {
            return DateTimeHelper.DateStringFromNow(this.PostView.CreateTime);
        }
    }
}
