using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Models
{
    /// <summary>
    /// 个人资料编辑模型
    /// </summary>
    public class ProfileInfoEditModel
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空")]
        [RegularExpression(@"^[\u4e00-\u9fa5a-zA-Z0-9]+$", ErrorMessage = "昵称只能为中文、字母、数字")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "昵称至少3个字符")]
        public string NickName { get; set; }
    }
}
