using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Models
{
    /// <summary>
    /// 修改密码模型
    /// </summary>
    public class ProfileModifyPasswordModel
    {
        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "当前密码")]
        [Required(ErrorMessage = "当前密码不能为空")]
        [RegularExpression("^[0-9a-zA-Z]+$", ErrorMessage = "必须为字母或数字")]
        [StringLength(20, ErrorMessage = "不少于6个字符", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "新密码")]
        [Required(ErrorMessage = "新密码不能为空")]
        [RegularExpression("^[0-9a-zA-Z]+$", ErrorMessage = "必须为字母或数字")]
        [StringLength(20, ErrorMessage = "不少于6个字符", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name = "确认新密码")]
        [Required(ErrorMessage = "确认新密码不能为空")]
        [Compare("NewPassword", ErrorMessage = "确认密码与新的密码输入不一致")]
        public string RepeatPassword { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码不能为空")]
        public string Code { get; set; }
    }
}
