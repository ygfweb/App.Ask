using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;

namespace App.Ask.Library.DAL
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }
        public async override Task<int> DeleteAsync(Comment entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await this.DeleteByIdAsync(entity.Id, transaction);
        }

        public async override Task<int> DeleteByIdAsync(Guid id, DbTransaction transaction = null)
        {
            string sql = "update comment set is_delete = true where id = @id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { id = id }, transaction);
        }

        /// <summary>
        /// 通过帖子ID和评论ID获取评论（通过此方法可以判断评论是否属于某个帖子）
        /// </summary>
        public async Task<Comment> GetByPostAndCommentId(Guid postId, Guid commentId)
        {
            string sql = "select * from comment where post_id = @postId and id = @id limit 1;";
            return await this.DbConnection.FirstOrDefaultAsync<Comment>(sql, new { postId = postId, id = commentId });
        }
        /// <summary>
        /// 仅修改评论内容
        /// </summary>
        public async Task<int> ModifyContentAsync(Guid commentId, string htmlContent, string textContent)
        {
            string sql = "update comment set html_content = @htmlContent,text_content=@textContent where id = @id";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { id = commentId, textContent = textContent, htmlContent = htmlContent });
        }

    }
}
