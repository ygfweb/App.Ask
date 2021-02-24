using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 评论表
    /// </summary>
    public class Comment : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 父评论
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 所属帖子
        /// </summary>
        [Index]
        public Guid PostId { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        [Index]
        public Guid PersonId { get; set; }

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

        /// <summary>
        /// 创建时间
        /// </summary>
        [Index]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeNum { get; set; }
    }
}

