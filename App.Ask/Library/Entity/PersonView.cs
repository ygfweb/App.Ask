using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 用户视图
    /// </summary>
    public class PersonView : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string AccountName { get; set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 角色
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 当前积分
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; } = "";

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = "";

        /// <summary>
        /// 提问数
        /// </summary>
        public int AskNum { get; set; } = 0;

        /// <summary>
        /// 发文数
        /// </summary>
        public int ArticleNum { get; set; } = 0;

        /// <summary>
        /// 回答数
        /// </summary>
        public int AnswerNum { get; set; } = 0;

        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeNum { get; set; } = 0;

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 是否禁言
        /// </summary>
        public bool IsMute { get; set; } = false;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; } = RoleType.User;
    }
}
