using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Info;
using App.Ask.Library.Services;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 评论服务
    /// </summary>
    public class CommentService
    {
        private readonly DbFactory dbFactory;
        private readonly HtmlService htmlService;

        public CommentService(DbFactory dbFactory, HtmlService htmlService)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.htmlService = htmlService ?? throw new ArgumentNullException(nameof(htmlService));
        }

        public async Task<List<CommentView>> GetCommentViewsAsync(Guid postId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.CommentView.GetWithPostAsync(postId);
            }
        }

        /// <summary>
        /// 插入评论
        /// </summary>
        public async Task<CommentView> InsertAsync(CommentEditModel model, Guid loginUserId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            string html = this.htmlService.ClearHtml(model.Content);
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new Exception("评论内容不能为空");
            }
            DateTime now = DateTime.Now;
            using (var work = this.dbFactory.StartWork())
            {
                Person person = await work.Person.SingleByIdAsync(loginUserId);
                if (person == null || person.IsDelete || person.IsMute)
                {
                    throw new Exception("该用户没有发贴权限");
                }
                Post post = await work.Post.SingleByIdAsync(model.PostId);
                if (post == null || post.PostStatus != Enums.PostStatus.Publish)
                {
                    throw new Exception("该帖子不存在，或禁止评论");
                }

                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        // 插入评论
                        Comment comment = new Comment
                        {
                            Id = GuidHelper.CreateSequential(),
                            CreateTime = now,
                            HtmlContent = html,
                            IsDelete = false,
                            LikeNum = 0,
                            ModifyTime = null,
                            ParentId = model.ParentId,
                            PersonId = loginUserId,
                            PostId = model.PostId,
                            TextContent = this.htmlService.HtmlToText(html)
                        };
                        await work.Comment.InsertAsync(comment, trans);
                        // 插入活动
                        CommentInfo info = new CommentInfo
                        {
                            CommentId = comment.Id,
                            PostId = post.Id,
                            PostTitle = post.Title
                        };
                        Activity activity = new Activity
                        {
                            Id = GuidHelper.CreateSequential(),
                            ActivityType = Enums.ActivityType.Comment,
                            DoTime = now,
                            PersonId = loginUserId,
                            PostId = post.Id
                        };
                        await work.Activity.InsertAsync(activity, trans);
                        // 递增用户回复数
                        await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.AnswerNum), nameof(PersonData.PersonId), person.Id, trans);
                        // 递增帖子回复数
                        await work.Connection.IncreaseAsync(nameof(Post), nameof(Post.CommentNum), nameof(Post.Id), post.Id, trans);
                        trans.Commit();
                        return await work.CommentView.SingleByIdAsync(comment.Id);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task DeleteAsync(Guid commentId, Guid loginUserId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView loginUser = await work.PersonView.GetByIdAsync(loginUserId);
                Comment comment = await work.Comment.SingleByIdAsync(commentId);
                if (comment != null)
                {
                    if (loginUser.Id == comment.PersonId || loginUser.RoleType == Enums.RoleType.Admin || loginUser.RoleType == Enums.RoleType.Master || loginUser.RoleType == Enums.RoleType.SuperAdmin)
                    {
                        await work.Comment.DeleteAsync(comment);
                    }
                    else
                    {
                        throw new Exception("没有权限删除该评论");
                    }
                }
            }
        }

        /// <summary>
        /// 通过ID获取评论
        /// </summary>
        public async Task<Comment> GetByIdAsync(Guid commentId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Comment.SingleByIdAsync(commentId);
            }
        }

        /// <summary>
        /// 修改评论内容
        /// </summary>
        public async Task<int> ModifyAsync(Guid commentId, string htmlContent, string textContent)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Comment.ModifyContentAsync(commentId, htmlContent, textContent);
            }
        }
    }
}
