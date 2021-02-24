using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Info;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 收藏服务
    /// </summary>
    public class FavoriteService
    {
        private readonly DbFactory dbFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FavoriteService(DbFactory dbFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// 检查当前用户是否已经收藏指定的帖子
        /// </summary>
        public async Task<bool> IsFavorite(Guid postId)
        {
            Guid? userId = this.httpContextAccessor.HttpContext.User.GetUserId();
            if (userId == null)
            {
                return false;
            }
            else
            {
                using (var work = this.dbFactory.StartWork())
                {
                    return await work.Favorite.IsFavorite(userId.Value, postId);
                }
            }
        }

        /// <summary>
        /// 为当前用户添加收藏
        /// </summary>
        public async Task<Favorite> InsertFavoriteAsync(Guid postId)
        {
            Guid? userId = this.httpContextAccessor.HttpContext.User.GetUserId();
            if (userId == null)
            {
                throw new Exception("使用收藏必须先登录");
            }
            else
            {
                using (var work = this.dbFactory.StartWork())
                {
                    Person person = await work.Person.SingleByIdAsync(userId.Value);
                    if (person == null || person.IsDelete)
                    {
                        throw new Exception("该用户不存在或已被删除");
                    }
                    Post post = await work.Post.SingleByIdAsync(postId);
                    if (post == null || post.PostStatus != Enums.PostStatus.Publish)
                    {
                        throw new Exception("该帖子不存在或未公开");
                    }
                    DateTime now = DateTime.Now;
                    using (var trans = work.BeginTransaction())
                    {
                        try
                        {
                            // 插入收藏
                            Favorite favorite = new Favorite
                            {
                                Id = GuidHelper.CreateSequential(),
                                PersonId = person.Id,
                                PostId = post.Id,
                                DoTime = now
                            };
                            await work.Favorite.InsertAsync(favorite, trans);
                            // 插入活动
                            Activity activity = new Activity
                            {
                                Id = GuidHelper.CreateSequential(),
                                ActivityType = Enums.ActivityType.Favorite,
                                DoTime = now,
                                PersonId = person.Id,
                                PostId = postId
                            };
                            await work.Activity.InsertAsync(activity);
                            trans.Commit();
                            return favorite;
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

        public async Task<PagingResult<PostView>> GetFavoritePagingResultAsync(Guid personId, int currentPage)
        {
            using (var work = this.dbFactory.StartWork())
            {
                try
                {
                    PageConfig pageConfig = await work.Config.GetConfigAsync<PageConfig>();
                    PagingResult<PostView> result = await work.Favorite.GetFavoritePagingResultAsync(personId, currentPage, pageConfig.PageSize);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
