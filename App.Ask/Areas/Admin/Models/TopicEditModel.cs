using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Areas.Admin.Models
{
    public class TopicEditModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "话题名称")]
        [Required(ErrorMessage = "话题名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Display(Name = "是否隐藏")]
        public bool IsHide { get; set; }

        [Display(Name = "系统保留")]
        public bool IsSystem { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "不能为空")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "必须是整数")]
        public int OrderNum { get; set; } = 0;
    }
}
