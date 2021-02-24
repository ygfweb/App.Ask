using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using SiHan.Libs.Utils.Text;
using SiHan.Libs.Utils.Time;

namespace App.Ask.Models
{
    /// <summary>
    /// 文章内容模型
    /// </summary>
    public class ArticleContentComponentModel
    {
        /// <summary>
        /// 文章作者
        /// </summary>
        public PersonView PublicPerson { get; set; }
        /// <summary>
        /// 登录用户
        /// </summary>
        public PersonView LoginPerson { get; set; }
        /// <summary>
        /// 文章
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// 文章数据
        /// </summary>
        public PostData PostData { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// 话题
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// 是否已点赞
        /// </summary>
        public bool IsLikePost { get; set; }

        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool IsFavoritePost { get; set; }

        /// <summary>
        /// 当前网址
        /// </summary>
        public string CurrentUrl { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin()
        {
            return this.LoginPerson != null && (LoginPerson.RoleType == RoleType.Admin || LoginPerson.RoleType == RoleType.Master || LoginPerson.RoleType == RoleType.SuperAdmin);
        }
        /// <summary>
        /// 是否是作者本人
        /// </summary>
        /// <returns></returns>
        public bool IsSelf()
        {
            return this.LoginPerson != null && this.LoginPerson.Id == this.PublicPerson.Id;
        }
        public string GetTime()
        {
            if (Post.ModifyTime != null)
            {
                return "修改于：" + DateTimeHelper.DateStringFromNow(Post.ModifyTime.Value);
            }
            else
            {
                return "发布于：" + DateTimeHelper.DateStringFromNow(Post.CreateTime);
            }
        }
    }
}
