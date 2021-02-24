using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 标签编辑模型
    /// </summary>
    public class TagEditModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "标签名称")]
        [Required(ErrorMessage = "标签名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "是否推荐")]
        public bool IsBest { get; set; } = true;

        public bool IsSystem { get; set; } = false;
    }
}
