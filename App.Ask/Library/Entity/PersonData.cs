using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public class PersonData : BaseEntity
    {
        public Guid Id { get; set; }

        [Index]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 当前积分
        /// </summary>
        public int Score { get; set; } = 0;
        /// <summary>
        /// 发文数
        /// </summary>
        public int ArticleNum { get; set; } = 0;

        /// <summary>
        /// 提问数
        /// </summary>
        public int AskNum { get; set; } = 0;

        /// <summary>
        /// 回答数
        /// </summary>
        [Index]
        public int AnswerNum { get; set; } = 0;

        /// <summary>
        /// 点赞数
        /// </summary>
        [Index]
        public int LikeNum { get; set; } = 0;
    }
}
