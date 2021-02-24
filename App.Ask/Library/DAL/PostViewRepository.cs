using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Paging;
using Microsoft.Extensions.Logging;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 帖子视图存储库
    /// </summary>
    public class PostViewRepository
    {
        private readonly DbConnection connection;

        public PostViewRepository(DbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<PagingResult<PostView>> GetPagingResultAsync(Guid topicId, PostResultType resultType = PostResultType.All, int currentPage = 1, int pageSize = 10, FilterType filter = FilterType.New)
        {
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view where");
            // 屏蔽被禁言用户的帖子
            builder.AppendToEnd("person_is_delete = false and person_is_mute = false");
            // 只能查询已发布的贴子
            builder.AppendToEnd("and post_status = @post_status").AddParameter("post_status", PostStatus.Publish);
            // 添加结果类型
            switch (resultType)
            {
                case PostResultType.All:
                    break;
                case PostResultType.Question:
                    builder.AppendToEnd("and post_type = @postType").AddParameter("postType", PostType.Question);
                    break;
                case PostResultType.Article:
                    builder.AppendToEnd("and post_type = @postType").AddParameter("postType", PostType.Article);
                    break;
            }

            // 添加话题
            if (topicId != Guid.Empty)
            {
                builder.AppendToEnd("and topic_id = @topicId").AddParameter("topicId", topicId);
            }
            // 过滤选项
            switch (filter)
            {
                case FilterType.Best:
                    builder.AppendToEnd("and is_best = true");
                    break;
                case FilterType.ZeroResponse:
                    builder.AppendToEnd("and comment_num = 0");
                    break;
            }
            // 总数查询
            SqlBuilder countQuery = builder.Clone() as SqlBuilder;
            countQuery.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countQuery);
            // 添加排序
            builder.InsertToStart("select *");
            if (filter == FilterType.Popular)
            {
                builder.AppendToEnd("order by is_top desc,comment_num desc,create_time desc");
            }
            else
            {
                builder.AppendToEnd("order by is_top desc,create_time desc");
            }
            // 添加分页
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.connection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }

        /// <summary>
        /// 获取查询字符串的帖子分页数据
        /// </summary>
        public async Task<PagingResult<PostView>> SearchAsync(Guid topicId, string search = "", int currentPage = 1, int pageSize = 10, FilterType filter = FilterType.New)
        {
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view where");
            // 屏蔽被禁言用户的帖子
            builder.AppendToEnd("person_is_delete = false and person_is_mute = false");
            // 只能查询已发布的贴子
            builder.AppendToEnd("and post_status = @post_status").AddParameter("post_status", PostStatus.Publish);
            // 添加搜索条件
            if (!string.IsNullOrWhiteSpace(search))
            {
                builder.AppendToEnd("and title like @title").AddParameter("title", "%" + search.Trim() + "%");
            }
            // 添加话题
            if (topicId != Guid.Empty)
            {
                builder.AppendToEnd("and topic_id = @topicId").AddParameter("topicId", topicId);
            }
            // 过滤选项
            switch (filter)
            {
                case FilterType.Best:
                    builder.AppendToEnd("and is_best = true");
                    break;
                case FilterType.ZeroResponse:
                    builder.AppendToEnd("and comment_num = 0");
                    break;
            }
            // 总数查询
            SqlBuilder countQuery = builder.Clone() as SqlBuilder;
            countQuery.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countQuery);
            // 添加排序
            builder.InsertToStart("select *");
            if (filter == FilterType.Popular)
            {
                builder.AppendToEnd("order by is_top desc,comment_num desc,create_time desc");
            }
            else
            {
                builder.AppendToEnd("order by is_top desc,create_time desc");
            }
            // 添加分页
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.connection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }

        /// <summary>
        /// 获取话题的帖子分页数据
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(Topic topic, int currentPage = 1, int pageSize = 10)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view where person_is_delete = false and person_is_mute = false and topic_id = @topicId");
            builder.AddParameter("topicId", topic.Id);
            // 屏蔽被禁言用户的帖子
            builder.AppendToEnd("person_is_delete = false and person_is_mute = false");
            // 只能查询已发布的贴子
            builder.AppendToEnd("and post_status = @post_status").AddParameter("post_status", PostStatus.Publish);
            // 总数
            SqlBuilder countBuilder = builder.Clone() as SqlBuilder;
            countBuilder.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countBuilder);
            //排序
            builder.AppendToEnd("order by is_top,create_time desc");
            // 分页
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.connection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }

        /// <summary>
        /// 获取标签的帖子分页数据
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(Tag tag, int currentPage = 1, int pageSize = 10, FilterType filter = FilterType.New)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view inner join post_tag on post_view.id = post_tag.post_id");
            builder.AppendToEnd("where person_is_delete = false and person_is_mute = false");
            builder.AppendToEnd("and post_tag.tag_id = @tagId").AddParameter("tagId", tag.Id);
            // 屏蔽被禁言用户的帖子
            builder.AppendToEnd("and person_is_delete = false and person_is_mute = false");
            // 只能查询已发布的贴子
            builder.AppendToEnd("and post_status = @post_status").AddParameter("post_status", PostStatus.Publish);
            // 过滤选项
            switch (filter)
            {
                case FilterType.Best:
                    builder.AppendToEnd("and is_best = true");
                    break;
                case FilterType.ZeroResponse:
                    builder.AppendToEnd("and comment_num = 0");
                    break;
            }
            //总数
            SqlBuilder countBuilder = builder.Clone() as SqlBuilder;
            countBuilder.InsertToStart("select count(post_view.*)");
            long count = await this.connection.ScalarAsync<long>(countBuilder);
            // 排序
            builder.InsertToStart("select *");
            builder.AppendToEnd("order by post_view.is_top,post_view.create_time desc");
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.connection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }

        /// <summary>
        /// 获取用户公开发表的的帖子
        /// </summary>
        public async Task<PagingResult<PostView>> GetPagingResultAsync(PersonView person, PostResultType resultType, int currentPage = 1, int pageSize = 10)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from post_view where person_is_mute = false and person_is_delete = false");
            builder.AppendToEnd("and post_status = @postStatus").AddParameter("postStatus", PostStatus.Publish);
            builder.AppendToEnd("and person_id =@personId").AddParameter("personId", person.Id);
            // 添加结果类型
            switch (resultType)
            {
                case PostResultType.Question:
                    builder.AppendToEnd("and post_type = @postType").AddParameter("postType", PostType.Question);
                    break;
                case PostResultType.Article:
                    builder.AppendToEnd("and post_type = @postType").AddParameter("postType", PostType.Article);
                    break;
            }
            // 计算总数
            SqlBuilder countBuilder = builder.Clone() as SqlBuilder;
            countBuilder.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countBuilder);
            // 获取记录
            builder.InsertToStart("select *");
            builder.AppendToEnd("order by post_view.is_top,post_view.create_time desc");
            builder.SetPaging(currentPage, pageSize);
            List<PostView> posts = await this.connection.SelectAsync<PostView>(builder);
            return new PagingResult<PostView>(currentPage, pageSize, count, posts);
        }

        /// <summary>
        /// 通过ID获取文章，包含被软删除的文章
        /// </summary>
        public async Task<PostView> GetById(Guid postId)
        {
            string sql = "select * from person_view where id = @id limit 1;";
            return await this.connection.FirstOrDefaultAsync<PostView>(sql, new { id = postId });
        }

        /// <summary>
        /// 获取最新公告
        /// </summary>
        public async Task<List<PostView>> GetNewAnnounceAsync()
        {
            string sql = "select * from post_view where topic_is_announce is true and post_status = @p order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { p = PostStatus.Publish });
        }

        /// <summary>
        /// 获取最新推荐的文章
        /// </summary>
        public async Task<List<PostView>> GetNewBestArticleAsync()
        {
            string sql = "select * from post_view where  topic_is_announce is false  and is_best is true  and post_type =@postType and post_status = @postStatus order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { postType = PostType.Article, postStatus = PostStatus.Publish });
        }
        /// <summary>
        /// 获取最新推荐的问题
        /// </summary>
        public async Task<List<PostView>> GetNewBestQuestionAsync()
        {
            string sql = "select * from post_view where topic_is_announce is false and is_best is true and post_type =@postType and post_status = @postStatus order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { postType = PostType.Question, postStatus = PostStatus.Publish });
        }
        /// <summary>
        /// 获取最新推荐的帖子（包含文章、提问）
        /// </summary>
        public async Task<List<PostView>> GetNewBestPostAsync()
        {
            string sql = "select * from post_view where topic_is_announce is false and is_best is true and post_status = @postStatus order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { postStatus = PostStatus.Publish });
        }

        /// <summary>
        /// 获取指定作者最新的文章
        /// </summary>
        public async Task<List<PostView>> GetNewArticleAsync(PersonView person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            string sql = "select * from post_view where topic_is_announce is false and post_type =@postType and post_status = @postStatus and person_id = @personId order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { postType = PostType.Article, postStatus = PostStatus.Publish, personId = person.Id });
        }
        /// <summary>
        /// 获取指定作者最新的问题
        /// </summary>
        public async Task<List<PostView>> GetNewBestQuestionAsync(PersonView person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            string sql = "select * from post_view where topic_is_announce is false and post_type =@postType and post_status = @postStatus and person_id = @personId order by create_time desc limit 5";
            return await this.connection.SelectAsync<PostView>(sql, new { postType = PostType.Question, postStatus = PostStatus.Publish, personId = person.Id });
        }
    }
}

