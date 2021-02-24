using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Models
{
    /// <summary>
    /// 公告编辑模型
    /// </summary>
    public class AnnounceEditModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string Content { get; set; }
    }
}