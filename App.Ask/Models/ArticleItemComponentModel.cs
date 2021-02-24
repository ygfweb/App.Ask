using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Text;
using SiHan.Libs.Utils.Time;

namespace App.Ask.Models
{
    /// <summary>
    /// 文章元素组件模型
    /// </summary>
    public class ArticleItemComponentModel
    {
        public PostView PostView { get; }
        public PostData PostData { get; }

        public ArticleItemComponentModel(PostView postView, PostData postData)
        {
            PostView = postView ?? throw new ArgumentNullException(nameof(postView));
            PostData = postData ?? throw new ArgumentNullException(nameof(postData));
        }

        public string getText()
        {
            return StringHelper.StringTruncat(this.PostData.TextContent, 80);
        }

        public string GetTime()
        {
            if (PostView.ModifyTime == null)
            {
                return "发布于：" + DateTimeHelper.DateStringFromNow(PostView.CreateTime);
            }
            else
            {
                return "修改于：" + DateTimeHelper.DateStringFromNow(PostView.ModifyTime.Value);
            }
        }
    }
}
