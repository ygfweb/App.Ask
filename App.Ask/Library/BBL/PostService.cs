using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Extensions;
using App.Ask.Library.Info;
using App.Ask.Library.Services;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Reflection;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 帖子服务类
    /// </summary>
    public class PostService
    {
        private readonly DbFactory dbFactory;
        private readonly HtmlService htmlService;
        private readonly IActionContextAccessor contextAccessor;

        public PostService(DbFactory dbFactory, HtmlService htmlService, IActionContextAccessor contextAccessor)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.htmlService = htmlService ?? throw new ArgumentNullException(nameof(htmlService));
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 创建新的帖子编辑模型
        /// </summary>
        public async Task<PostEditModel> CreateNewEditModelAsync(PostType defaultType, PersonView person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            using (var work = this.dbFactory.StartWork())
            {
                PostEditModel model = new PostEditModel();
                model.PostType = defaultType;
                // 创建话题列表
                model.TopicSelectItems.Add(new SelectListItem("选择话题分类...", ""));
                List<Topic> topics = new List<Topic>();
                if (person.RoleType == RoleType.Admin || person.RoleType == RoleType.SuperAdmin)
                {
                    topics = await work.Topic.GetAllAsync(SearchType.Visible, true);
                }
                else
                {
                    topics = await work.Topic.GetAllAsync(SearchType.Visible, false);
                }
                foreach (var topic in topics)
                {
                    model.TopicSelectItems.Add(new SelectListItem(topic.Name, topic.Id.ToString()));
                };
                List<string> BestTags = (await work.Tag.GetBestAsync()).Select(p => p.Name).ToList();
                model.BestTags = JsonConvert.SerializeObject(BestTags);
                return model;
            }
        }

        public async Task<PostEditModel> CreateNewEditModelAsync(Post post, PersonView person)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            PostEditModel model = await CreateNewEditModelAsync(post.PostType, person);
            using (var work = this.dbFactory.StartWork())
            {
                List<Tag> tags = await work.PostTag.GetTagsByPostAsync(post);
                PostData postData = await work.PostData.GetPostDataAsync(post);
                model.Content = postData.HtmlContent;
                model.Title = post.Title;
                model.TopicId = post.TopicId;
                model.Id = post.Id;
                model.UseTags = work.Tag.ToStringList(tags);
                model.UseTagSelectItems = work.Tag.ToSelectListItems(tags);
            }
            return model;
        }

        /// <summary>
        /// 插入帖子（问题、文章）
        /// </summary>
        public async Task<Post> InsertAsync(PostEditModel model, Guid userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.UseTags.Count > 5)
            {
                throw new ModelException(nameof(model.UseTags), "最多只能创建5个标签");
            }
            string htmlContent = htmlService.ClearHtml(model.Content);
            string textContent = htmlService.HtmlToText(htmlContent);
            if (model.PostType == PostType.Article && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new Exception("文章内容不能为空");
            }
            PostStatus postStatus = PostStatus.Publish;
            DateTime now = DateTime.Now;
            Post post = new Post()
            {
                Id = GuidHelper.CreateSequential(),
                CreateTime = now,
                IsBest = false,
                IsTop = false,
                ModifyTime = null,
                PostStatus = postStatus,
                PostType = model.PostType, // 帖子类型
                Title = htmlService.HtmlToText(model.Title),
                TopicId = model.TopicId.Value,
                PersonId = userId
            };
            PostData postData = new PostData
            {
                Id = GuidHelper.CreateSequential(),
                HtmlContent = htmlContent,
                PostId = post.Id,
                TextContent = textContent
            };
            Activity activity = new Activity
            {
                Id = GuidHelper.CreateSequential(),
                DoTime = now,
                PersonId = userId,
                ActivityType = ActivityType.Publish,
                PostId = post.Id
            };

            using (var work = this.dbFactory.StartWork())
            {
                Person person = await work.Person.SingleByIdAsync(userId);
                if (person == null || person.IsDelete || person.IsMute)
                {
                    throw new Exception("你没有发帖权限");
                }
                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        List<string> tags = work.Tag.RemoveDuplication(model.UseTags);
                        foreach (var item in tags)
                        {
                            Tag tag = await work.Tag.GetByNameAsync(item);
                            // 如果标签不存在，则重新创建
                            if (tag == null)
                            {
                                tag = new Tag
                                {
                                    Id = GuidHelper.CreateSequential(),
                                    IsBest = false,
                                    IsSystem = false,
                                    Name = item.Trim()
                                };
                                await work.Tag.InsertAsync(tag, trans);
                            }
                            // 记录帖子与标签之间的关联
                            PostTag postTag = new PostTag
                            {
                                Id = GuidHelper.CreateSequential(),
                                PostId = post.Id,
                                TagId = tag.Id
                            };
                            await work.PostTag.InsertAsync(postTag, trans);
                        }
                        await work.Post.InsertAsync(post, trans); // 插入文章
                        await work.PostData.InsertAsync(postData, trans); // 插入文章内容
                        await work.Activity.InsertAsync(activity, trans); // 插入活动
                        if (post.PostType == PostType.Question)
                        {
                            await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.AskNum), nameof(PersonData.PersonId), person.Id, trans); // 递增提问数
                        }
                        else if (post.PostType == PostType.Article)
                        {
                            await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.ArticleNum), nameof(PersonData.PersonId), person.Id, trans); // 递增发文数
                        }
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return post;
        }

        /// <summary>
        /// 创建公告
        /// </summary>
        public async Task<Post> CreateAnnounceAsync(AnnounceEditModel model, PersonView person)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            string htmlContent = htmlService.ClearHtml(model.Content);
            string textContent = htmlService.HtmlToText(htmlContent);
            if (string.IsNullOrWhiteSpace(textContent))
            {
                throw new Exception("公告内容不能为空");
            }
            using (var work = this.dbFactory.StartWork())
            {
                if (person.RoleType != RoleType.Admin && person.RoleType != RoleType.SuperAdmin)
                {
                    throw new Exception("你没有发布公告权限");
                }
                // 获取公告话题
                Topic topic = await work.Topic.GetAnnounce();
                DateTime now = DateTime.Now;
                Post post = new Post()
                {
                    Id = GuidHelper.CreateSequential(),
                    CreateTime = now,
                    IsBest = false,
                    IsTop = false,
                    ModifyTime = null,
                    PostStatus = PostStatus.Publish,
                    PostType = PostType.Article, // 帖子类型
                    Title = htmlService.HtmlToText(model.Title),
                    TopicId = topic.Id,
                    PersonId = person.Id
                };
                PostData postData = new PostData
                {
                    Id = GuidHelper.CreateSequential(),
                    HtmlContent = htmlContent,
                    TextContent = textContent,
                    PostId = post.Id
                };
                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        await work.Post.InsertAsync(post, trans); // 插入文章
                        await work.PostData.InsertAsync(postData, trans); // 插入文章内容
                        await work.Connection.IncreaseAsync(nameof(PersonData), nameof(PersonData.ArticleNum), nameof(PersonData.PersonId), person.Id, trans); // 递增发文数
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                return post;
            }
        }

        /// <summary>
        /// 修改帖子
        /// </summary>
        public async Task ModifyAsync(PostEditModel model, Guid userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }
            if (model.UseTags.Count > 5)
            {
                throw new ModelException(nameof(model.UseTags), "最多只能创建5个标签");
            }
            string htmlContent = htmlService.ClearHtml(model.Content);
            string textContent = htmlService.HtmlToText(htmlContent);
            if (model.PostType == PostType.Article && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new Exception("文章内容不能为空");
            }
            string title = htmlService.HtmlToText(model.Title).Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ModelException(nameof(model.Title), "标题不能为空");
            }
            using (var work = this.dbFactory.StartWork())
            {
                Post post = await work.Post.SingleByIdAsync(model.Id.Value);
                if (post == null || post.PostStatus == PostStatus.Block)
                {
                    throw new Exception("该帖子不存在或已被删除");
                }
                if (userId != post.PersonId)
                {
                    throw new Exception("仅本人拥有修改的权限");
                }
                PostData postData = await work.PostData.GetPostDataAsync(post);
                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        await work.PostTag.DeleteAllAsync(post, trans);
                        List<string> tags = work.Tag.RemoveDuplication(model.UseTags);
                        foreach (var item in tags)
                        {
                            Tag tag = await work.Tag.GetByNameAsync(item);
                            // 如果标签不存在，则重新创建
                            if (tag == null)
                            {
                                tag = new Tag
                                {
                                    Id = GuidHelper.CreateSequential(),
                                    IsBest = false,
                                    IsSystem = false,
                                    Name = item.Trim()
                                };
                                await work.Tag.InsertAsync(tag, trans);
                            }
                            // 记录帖子与标签之间的关联
                            PostTag postTag = new PostTag
                            {
                                Id = GuidHelper.CreateSequential(),
                                PostId = post.Id,
                                TagId = tag.Id
                            };
                            await work.PostTag.InsertAsync(postTag, trans);
                        }
                        DateTime now = DateTime.Now;
                        // 修改帖子基本信息
                        await work.Post.ModifyContent(post.Id, htmlService.HtmlToText(model.Title), model.TopicId.Value, now, trans);
                        // 修改帖子内容
                        postData.HtmlContent = htmlContent;
                        postData.TextContent = textContent;
                        await work.PostData.UpdateAsync(postData);
                        // 插入活动
                        Activity activity = new Activity
                        {
                            Id = GuidHelper.CreateSequential(),
                            ActivityType = ActivityType.Modify,
                            PersonId = userId,
                            PostId = post.Id,
                            DoTime = now
                        };
                        await work.Activity.InsertAsync(activity, trans);
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

        /// <summary>
        /// 搜索帖子，如果字符串为空，则显示所有帖子
        /// </summary>
        public async Task<PagingResult<PostView>> SearchAsync(Guid topicId, FilterType filter = FilterType.New, string search = "", int page = 1)
        {
            using (var work = this.dbFactory.StartWork())
            {
                var config = await work.Config.GetPageConfigAsync();
                return await work.PostView.SearchAsync(topicId, search, page, config.PageSize, filter);
            }
        }

        /// <summary>
        /// 通过帖子ID获取帖子视图
        /// </summary>
        public async Task<PostView> GetPostViewByIdAsync(Guid postId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetById(postId);
            }
        }
        /// <summary>
        /// 通过帖子ID获取帖子
        /// </summary>
        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.SingleByIdAsync(postId);
            }
        }

        /// <summary>
        /// 获取帖子类型
        /// </summary>
        public async Task<PostType?> GetPostTypeAsync(Guid postId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.GetPostTypeAsync(postId);
            }
        }

        /// <summary>
        /// 获取帖子内容
        /// </summary>
        public async Task<PostData> GetPostDataAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostData.GetPostDataAsync(post);
            }
        }
        /// <summary>
        /// 设置为精华帖
        /// </summary>
        public async Task<int> SetBestAsync(Guid postId, bool isBest)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.SetBestAsync(postId, isBest);
            }
        }
        /// <summary>
        /// 置顶帖子
        /// </summary>
        public async Task<int> SetTopAsync(Guid postId, bool isTop)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Post.SetTopAsync(postId, isTop);
            }
        }

        /// <summary>
        /// 屏蔽帖子
        /// </summary>
        public async Task<int> BlockAsync(Guid postId, PersonView doPerson)
        {
            using (var work = this.dbFactory.StartWork())
            {
                Post post = await work.Post.SingleByIdAsync(postId);
                if (post == null)
                {
                    throw new Exception("该帖子不存在");
                }
                PersonView person = await work.PersonView.GetByIdAsync(post.PersonId);
                if (doPerson.RoleType > person.RoleType)
                {
                    throw new Exception("该操作权限不足");
                }
                return await work.Post.BlockAsync(postId);
            }
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(Guid topicId, PostResultType resultType = PostResultType.All, int currentPage = 1, FilterType filter = FilterType.New)
        {
            using (var work = this.dbFactory.StartWork())
            {
                var config = await work.Config.GetPageConfigAsync();
                return await work.PostView.GetPagingResultAsync(topicId, resultType, currentPage, config.PageSize, filter);
            }
        }

        /// <summary>
        /// 获取用户公开发表的帖子
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(PersonView person, PostResultType resultType, int currentPage)
        {
            using (var work = this.dbFactory.StartWork())
            {
                var config = await work.Config.GetPageConfigAsync();
                return await work.PostView.GetPagingResultAsync(person, resultType, currentPage, config.PageSize);
            }
        }

        /// <summary>
        /// 获取标签的帖子
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(Tag tag, int currentPage = 1, FilterType filter = FilterType.New)
        {
            using (var work = this.dbFactory.StartWork())
            {
                var config = await work.Config.GetPageConfigAsync();
                return await work.PostView.GetPagingResultAsync(tag, currentPage, config.PageSize, filter);
            }
        }

        /// <summary>
        /// 递增文章的阅读数
        /// </summary>
        public async Task IncreaseViewNumAsync(Post post)
        {
            using (var work = this.dbFactory.StartWork())
            {
                await work.Connection.IncreaseAsync(nameof(Post), nameof(Post.ViewNum), nameof(Post.Id), post.Id, null);
            }
        }

        /// <summary>
        /// 获取最新公告(最多5条数据)
        /// </summary>
        public async Task<List<PostView>> GetNewAnnounceAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewAnnounceAsync();
            }
        }

        /// <summary>
        /// 获取最新推荐的文章
        /// </summary>
        public async Task<List<PostView>> GetNewBestArticleAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewBestArticleAsync();
            }
        }

        /// <summary>
        /// 获取作者最新的文章
        /// </summary>
        public async Task<List<PostView>> GetNewBestArticleAsync(PersonView person)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewArticleAsync(person);
            }
        }

        /// <summary>
        /// 获取最新推荐的提问
        /// </summary>
        public async Task<List<PostView>> GetNewBestQuestionAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewBestQuestionAsync();
            }
        }

        /// <summary>
        /// 获取最新推荐的提问
        /// </summary>
        public async Task<List<PostView>> GetNewBestQuestionAsync(PersonView person)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewBestQuestionAsync(person);
            }
        }

        /// <summary>
        /// 获取最新推荐的帖子（包含文章、提问）
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostView>> GetNewBestPostAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostView.GetNewBestPostAsync();
            }
        }


    }
}