using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;

namespace App.Ask.Library.DAL
{
    public class CommentViewRepository
    {
        private readonly DbConnection connection;

        public CommentViewRepository(DbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// 获取帖子的所有评论
        /// </summary>
        public async Task<List<CommentView>> GetWithPostAsync(Guid postId)
        {
            string sql = "select * from comment_view where post_id = @postId and is_delete is false order by create_time;";
            return await this.connection.SelectAsync<CommentView>(sql, new { postId = postId });
        }

        /// <summary>
        /// 获取评论视图
        /// </summary>
        public async Task<CommentView> SingleByIdAsync(Guid commentId)
        {
            string sql = "select * from comment_view where id = @id;";
            return await this.connection.FirstOrDefaultAsync<CommentView>(sql, new { id = commentId });
        }
    }
}
