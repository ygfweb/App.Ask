using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        public DbConnection Connection { get; }

        public UnitOfWork(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.Person = new PersonRepository(this.Connection);
            this.PersonData = new PersonDataRepository(this.Connection);
            this.Role = new RoleRepository(this.Connection);
            this.Config = new ConfigRepository(this.Connection);
            this.InviteCode = new InviteCodeRepository(this.Connection);
            this.Topic = new TopicRepository(this.Connection);
            this.Tag = new TagRepository(this.Connection);
            this.Post = new PostRepository(this.Connection);
            this.PostView = new PostViewRepository(this.Connection);
            this.PostData = new PostDataRepository(this.Connection);
            this.PostTag = new PostTagRepository(this.Connection);
            this.Person = new PersonRepository(this.Connection);
            this.PersonView = new PersonViewRepository(this.Connection);
            this.Activity = new ActivityRepository(this.Connection);
            this.Comment = new CommentRepository(this.Connection);
            this.CommentView = new CommentViewRepository(this.Connection);
            this.Zan = new ZanRepository(this.Connection);
            this.Favorite = new FavoriteRepository(this.Connection);
            this.ActivityView = new ActivityViewRepository(this.Connection);
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public DbTransaction BeginTransaction()
        {
            return this.Connection.BeginTransaction();
        }
        /// <summary>
        /// 用户存储库
        /// </summary>
        public PersonRepository Person { get; }
        /// <summary>
        /// 用户数据存储库
        /// </summary>
        public PersonDataRepository PersonData { get; }
        /// <summary>
        /// 角色存储库
        /// </summary>
        public RoleRepository Role { get; }

        /// <summary>
        /// 系统配置储存库
        /// </summary>
        public ConfigRepository Config { get; }
        /// <summary>
        /// 邀请码存储库
        /// </summary>
        public InviteCodeRepository InviteCode { get; }
        /// <summary>
        /// 话题存储库
        /// </summary>
        public TopicRepository Topic { get; }

        /// <summary>
        /// 帖子存储库
        /// </summary>
        public PostRepository Post { get; }

        public PostDataRepository PostData { get; }
        /// <summary>
        /// 标签存储库
        /// </summary>
        public TagRepository Tag { get; }
        /// <summary>
        /// 帖子标签关联存储库
        /// </summary>
        public PostTagRepository PostTag { get; }

        /// <summary>
        /// 用户视图
        /// </summary>
        public PersonViewRepository PersonView { get; }
        /// <summary>
        /// 活动储存库
        /// </summary>
        public ActivityRepository Activity { get; }
        /// <summary>
        /// 帖子视图
        /// </summary>
        public PostViewRepository PostView { get; }
        /// <summary>
        /// 评论视图
        /// </summary>
        public CommentViewRepository CommentView { get; }
        /// <summary>
        /// 评论
        /// </summary>
        public CommentRepository Comment { get; }

        /// <summary>
        /// 收藏夹
        /// </summary>
        public FavoriteRepository Favorite { get; }

        /// <summary>
        /// 点赞存储库
        /// </summary>
        public ZanRepository Zan { get; }

        /// <summary>
        /// 活动视图储存库
        /// </summary>
        public ActivityViewRepository ActivityView { get; set; }

        public void Dispose()
        {
            this.Connection.Dispose();
        }
    }
}
