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
    /// <summary>
    /// 帖子标签关联存储库
    /// </summary>
    public class PostTagRepository : BaseRepository<PostTag>
    {
        public PostTagRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }


        /// <summary>
        /// 获取帖子的所有标签
        /// </summary>
        public async Task<List<Tag>> GetTagsByPostAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            string sql = @"select tag.* from tag inner join post_tag on tag.id = post_tag.tag_id inner join post on post_tag.post_id = post.id where post.id =@postId;";
            return await this.DbConnection.SelectAsync<Tag>(sql, new { postId = post.Id });
        }
        /// <summary>
        /// 获取帖子的所有标签
        /// </summary>
        public async Task<List<Tag>> GetTagsByPostAsync(Guid postId)
        {
            string sql = @"select tag.* from tag inner join post_tag on tag.id = post_tag.tag_id inner join post on post_tag.post_id = post.id where post.id =@postId;";
            return await this.DbConnection.SelectAsync<Tag>(sql, new { postId = postId });
        }

        /// <summary>
        /// 删除帖子中的所有标签
        /// </summary>
        public async Task DeleteAllAsync(Post post, DbTransaction trans)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            string sql = "delete from post_tag where post_id = @id";
            await this.DbConnection.ExecuteNonQueryAsync(sql, new { id = post.Id }, trans);
        }

        /// <summary>
        /// 获取帖子的所有标签关联
        /// </summary>
        public async Task<List<PostTag>> GetByPostAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            string sql = "select * from post_tag where post_id = @postId";
            return await this.DbConnection.SelectAsync<PostTag>(sql, new { postId = post.Id });
        }

        /// <summary>
        /// 获取该标签的所有帖子总数
        /// </summary>
        public async Task<long> GetPostCountAsync(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            string sql = "select  count(post.*) from post inner join post_tag on post.id = post_tag.post_id inner join tag on post_tag.tag_id = tag.id where tag.id = @tagId;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { tagId = tag.Id });
        }

        /// <summary>
        /// 获取该标签指定状态的帖子总数
        /// </summary>
        public async Task<long> GetPostCountAsync(Tag tag, PostStatus status = PostStatus.Publish)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            string sql = "select  count(post.*) from post inner join post_tag on post.id = post_tag.post_id inner join tag on post_tag.tag_id = tag.id where tag.id = @tagId and post_status = @postStatus;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { tagId = tag.Id, postStatus = status });
        }

        /// <summary>
        /// 删除指定标签的所有关联
        /// </summary>
        public async Task<int> DeleteByTagAsync(Tag tag, DbTransaction transaction = null)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            string sql = "delete from post_tag where tag_id = @tagId";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { tagId = tag.Id }, transaction);
        }
    }
}