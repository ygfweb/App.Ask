using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Ask.Models
{
    public class PostEditModel
    {
        public Guid? Id { get; set; } = null;

        [Display(Name = "标题")]
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(50, ErrorMessage = "最长为20个字符")]
        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        [Display(Name = "所属话题")]
        [Required(ErrorMessage = "所属话题不能为空")]
        public Guid? TopicId { get; set; }

        [Display(Name = "帖子类型")]
        [Required(ErrorMessage = "帖子类型不能为空")]
        public PostType PostType { get; set; }

        /// <summary>
        /// 所属话题列表
        /// </summary>
        public List<SelectListItem> TopicSelectItems { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// 拥有标签
        /// </summary>
        [Display(Name = "标签")]
        public List<string> UseTags { get; set; } = new List<string>();

        /// <summary>
        /// 拥有标签
        /// </summary>
        public List<SelectListItem> UseTagSelectItems { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// 推荐标签
        /// </summary>
        public string BestTags { get; set; } = "[]";

        public static PostEditModel Create(Post post, PostData postData, List<Tag> useTags, List<Tag> bestTags)
        {
            PostEditModel model = new PostEditModel();
            if (post != null)
            {
                model.Id = post.Id;
                model.Title = post.Title;
                model.Content = postData.HtmlContent;
                model.TopicId = post.TopicId;
                model.PostType = post.PostType;
            }
            return model;
        }
    }
}
