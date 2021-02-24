using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 帖子数据
    /// </summary>
    public class PostData : BaseEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 帖子ID
        /// </summary>
        [Index]
        public Guid PostId { get; set; }
        /// <summary>
        /// HTML内容
        /// </summary>
        [Column(DbType = "text")]
        public string HtmlContent { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        [Column(DbType = "text")]
        public string TextContent { get; set; }
    }
}
