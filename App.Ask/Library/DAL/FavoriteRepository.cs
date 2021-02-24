using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Library.DAL
{
    public class FavoriteRepository : BaseRepository<Favorite>
    {
        public FavoriteRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// 检查用户是否收藏帖子
        /// </summary>
        public async Task<bool> IsFavorite(Guid personId, Guid postId)
        {
            string sql = "select count(*) from favorite where person_id =@personId and post_id = @postId;";
            long count = await this.DbConnection.ScalarAsync<long>(sql, new { personId = personId, postId = postId });
            return count > 0;
        }

        /// <summary>
        /// 获取个人收藏夹
        /// </summary>
        public async Task<PagingResult<PostView>> GetFavoritePagingResultAsync(Guid personId, int currentPage, int pageSize)
        {
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view,favorite where post_view.id = favorite.post_id and post_view.post_status = @postStatus and favorite.person_id = @personId");
            builder.AddParameter("postStatus", PostStatus.Publish).AddParameter("personId", personId);
            // 计算总数
            SqlBuilder countBuilder = builder.Clone() as SqlBuilder;
            countBuilder.InsertToStart("select count(post_view.*)");
            long count = await this.DbConnection.ScalarAsync<long>(countBuilder);
            // 获取记录
            builder.InsertToStart("select post_view.*");
            builder.AppendToEnd("order by favorite.do_time desc");
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.DbConnection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }
    }
}
