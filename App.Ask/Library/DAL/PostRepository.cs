using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 帖子储存库
    /// </summary>
    public class PostRepository : BaseRepository<Post>
    {
        public PostRepository(DbConnection connection) : base(connection) { }

        /// <summary>
        /// 获取指定话题的帖子总数
        /// </summary>
        public async Task<long> GetCountByTopicAsync(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            string sql = "SELECT count(*) FROM post WHERE topic_id =  @topicId;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { topicId = topic.Id });
        }

        /// <summary>
        /// 获取话题内指定状态的贴子总数
        /// </summary>
        public async Task<long> GetCountByTopicAsync(Topic topic, PostStatus status)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            string sql = "SELECT count(*) FROM post Where topic_id = @topicId AND post_status = @postStatus;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { topicId = topic.Id, postStatus = status });
        }

        /// <summary>
        /// 获取帖子类型
        /// </summary>
        public async Task<PostType> GetPostTypeAsync(Guid postId)
        {
            string sql = "select post_type from post where id= @id;";
            short result = await this.DbConnection.ScalarAsync<short>(sql, new { id = postId });
            return (PostType)result;
        }

        /// <summary>
        /// 设置为精华帖，false表示取消
        /// </summary>
        public async Task<int> SetBestAsync(Guid postId, bool isBest, DbTransaction trans = null)
        {
            string sql = "update post set is_best = @isBest where id=@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { isBest = isBest, id = postId }, trans);
        }

        /// <summary>
        /// 置顶帖，false表示取消
        /// </summary>
        public async Task<int> SetTopAsync(Guid postId, bool isTop, DbTransaction trans = null)
        {
            string sql = "update post set is_top = @isTop where id=@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { isTop = isTop, id = postId }, trans);
        }
        /// <summary>
        /// 修改帖子内容
        /// </summary>
        public async Task<int> ModifyContent(Guid postId, string title, Guid topicId, DateTime modifyTime, DbTransaction trans)
        {
            string sql = "update post set title = @title,topic_id=@topicid,modify_time=@modifyTime where id =@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { title = title, topicId = topicId, modifyTime = modifyTime, id = postId }, trans);
        }

        /// <summary>
        /// 屏蔽帖子
        /// </summary>
        public async Task<int> BlockAsync(Guid postId, DbTransaction trans = null)
        {
            string sql = "update post set post_status = @postStatus where id = @id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { postStatus = PostStatus.Block, id = postId }, trans);
        }

        /// <summary>
        /// 获取文章总数
        /// </summary>
        public async Task<long> GetArticleCountAsync()
        {
            string sql = "select count(*) from post where post_type = @postType;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { postType = PostType.Article });
        }

        /// <summary>
        /// 获取问答总数
        /// </summary>
        public async Task<long> GetQuestionCountAsync()
        {
            string sql = "select count(*) from post where post_type = @postType;";
            return await this.DbConnection.ScalarAsync<long>(sql, new { postType = PostType.Question });
        }
    }
}