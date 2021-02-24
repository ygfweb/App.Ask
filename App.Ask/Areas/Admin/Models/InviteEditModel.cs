using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 邀请码编辑模型
    /// </summary>
    public class InviteEditModel
    {
        [Display(Name = "邀请码")]
        [Required(ErrorMessage = "邀请码不能为空")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "必须为字母或数字")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "至少3个字符")]
        public string Code { get; set; }
    }
}
