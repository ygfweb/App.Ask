using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Info;
using App.Ask.Library.Utils;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 点赞服务
    /// </summary>
    public class ZanService
    {
        private readonly DbFactory dbFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ZanService(DbFactory dbFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        /// <summary>
        /// 判断当前用户是否对帖子点赞
        /// </summary>
        public async Task<bool> IsPostZanAsync(Guid postId)
        {
            Guid? personId = httpContextAccessor.HttpContext.User.GetUserId();
            using (var work = this.dbFactory.StartWork())
            {
                if (personId == null)
                {
                    // 匿名用户检查session ID
                    string sessionId = httpContextAccessor.HttpContext.Session.Id;
                    return await work.Zan.IsZanAsync(sessionId, postId, Enums.ZanType.Post);
                }
                else
                {
                    // 登录用户检查用户ID
                    return await work.Zan.IsZanAsync(personId.Value, postId, Enums.ZanType.Post);
                }
            }
        }
        /// <summary>
        /// 判断当前登录用户是否对评论点赞
        /// </summary>
        public async Task<bool> IsCommentZanAsync(Guid commentId)
        {
            Guid? personId = httpContextAccessor.HttpContext.User.GetUserId();
            using (var work = this.dbFactory.StartWork())
            {
                if (personId == null)
                {
                    // 匿名用户检查session ID
                    string sessionId = httpContextAccessor.HttpContext.Session.Id;
                    return await work.Zan.IsZanAsync(sessionId, commentId, Enums.ZanType.Comment);
                }
                else
                {
                    // 登录用户检查用户ID
                    return await work.Zan.IsZanAsync(personId.Value, commentId, Enums.ZanType.Comment);
                }
            }
        }

        /// <summary>
        /// 当前用户对帖子点赞
        /// </summary>
        public async Task ZanPostAsync(Guid postId)
        {
            Guid? personId = httpContextAccessor.HttpContext.User.GetUserId();
            string sessionId = httpContextAccessor.HttpContext.Session.Id;
            Zan zan = null;
            DateTime now = DateTime.Now;
            using (var work = this.dbFactory.StartWork())
            {
                Post post = await work.Post.SingleByIdAsync(postId);
                if (post == null || post.PostStatus != Enums.PostStatus.Publish)
                {
                    throw new Exception("该帖子不存在或已被删除");
                }
                // 如果是匿名用户
                if (personId == null)
                {
                    if (!(await work.Zan.IsZanAsync(sessionId, postId, Enums.ZanType.Post)))
                    {
                        //添加赞记录
                        zan = new Zan
                        {
                            Id = GuidHelper.CreateSequential(),
                            SessionId = this.httpContextAccessor.HttpContext.Session.Id,
                            CommentId = null,
                            DoTime = now,
                            PersonId = null,
                            PostId = postId,
                            ZanType = Enums.ZanType.Post
                        };
                    }
                }
                else
                {
                    // 如果是登录用户
                    if (!(await work.Zan.IsZanAsync(personId.Value, postId, Enums.ZanType.Post)))
                    {
                        zan = new Zan
                        {
                            Id = GuidHelper.CreateSequential(),
                            SessionId = this.httpContextAccessor.HttpContext.Session.Id,
                            CommentId = null,
                            DoTime = now,
                            PersonId = personId.Value,
                            PostId = postId,
                            ZanType = Enums.ZanType.Post
                        };
                    }
                }
                if (zan != null)
                {
                    using (var trans = work.BeginTransaction())
                    {
                        try
                        {
                            await work.Zan.InsertAsync(zan, trans);
                            // 累计帖子赞
                            await work.Connection.IncreaseAsync(nameof(Post), nameof(Post.LikeNum), nameof(Post.Id), postId, trans);
                            // 累计作者赞
                            await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.LikeNum), nameof(PersonData.PersonId), post.PersonId, trans);
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 当前用户对评论点赞
        /// </summary>
        public async Task ZanCommentAsync(Guid commentId)
        {
            Guid? personId = httpContextAccessor.HttpContext.User.GetUserId();
            string sessionId = httpContextAccessor.HttpContext.Session.Id;
            Zan zan = null;
            DateTime now = DateTime.Now;
            using (var work = this.dbFactory.StartWork())
            {
                Comment comment = await work.Comment.SingleByIdAsync(commentId);
                if (comment == null || comment.IsDelete)
                {
                    throw new Exception("该评论不存在或已被删除");
                }
                // 如果是匿名用户
                if (personId == null)
                {
                    if (!(await work.Zan.IsZanAsync(sessionId, commentId, Enums.ZanType.Comment)))
                    {
                        //添加赞记录
                        zan = new Zan
                        {
                            Id = GuidHelper.CreateSequential(),
                            SessionId = this.httpContextAccessor.HttpContext.Session.Id,
                            CommentId = comment.Id,
                            DoTime = now,
                            PersonId = null,
                            PostId = comment.PostId,
                            ZanType = Enums.ZanType.Comment
                        };
                    }
                }
                else
                {
                    // 如果是登录用户
                    if (!(await work.Zan.IsZanAsync(personId.Value, commentId, Enums.ZanType.Comment)))
                    {
                        zan = new Zan
                        {
                            Id = GuidHelper.CreateSequential(),
                            SessionId = this.httpContextAccessor.HttpContext.Session.Id,
                            CommentId = comment.Id,
                            DoTime = now,
                            PersonId = personId.Value,
                            PostId = comment.PostId,
                            ZanType = Enums.ZanType.Comment
                        };
                    }
                }
                if (zan != null)
                {
                    using (var trans = work.BeginTransaction())
                    {
                        try
                        {
                            await work.Zan.InsertAsync(zan, trans);
                            // 累计评论赞
                            await work.Connection.IncreaseAsync(nameof(Comment), nameof(Comment.LikeNum), nameof(Comment.Id), commentId, trans);
                            // 累计作者赞
                            await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.LikeNum), nameof(PersonData.PersonId), comment.PersonId, trans);
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
