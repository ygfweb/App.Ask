using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Time;

namespace App.Ask.Models
{
    /// <summary>
    /// 活动元素组件模型
    /// </summary>
    public class ActivityItemComponentModel
    {
        public ActivityView ActivityView { get; set; }

        public string GetPublishTime()
        {
            return DateTimeHelper.DateStringFromNow(this.ActivityView.DoTime);
        }
    }
}
