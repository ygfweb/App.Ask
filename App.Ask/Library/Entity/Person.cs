using System;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class Person : BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Index]
        public string AccountName { get; set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        [Index]
        public string NickName { get; set; } = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 角色
        /// </summary>
        [Index]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;

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
        public string Avatar { get; set; } = GlobalVariable.DefaultAvatar;
        /// <summary>
        /// 是否删除
        /// </summary>
        [Index]
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 是否禁言
        /// </summary>
        [Index]
        public bool IsMute { get; set; } = false;
    }
}