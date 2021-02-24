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
    /// 话题存储库
    /// </summary>
    public class TopicRepository : BaseRepository<Topic>
    {
        public TopicRepository(DbConnection connection) : base(connection) { }
        /// <summary>
        /// 通过名称获取话题
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Topic> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            string sql = "SELECT * FROM topic WHERE upper(name) = @name LIMIT 1;";
            Topic topic = await this.DbConnection.FirstOrDefaultAsync<Topic>(sql, new { name = name.ToUpper() });
            return topic;
        }

        /// <summary>
        /// 获取所有话题
        /// </summary>
        public async Task<List<Topic>> GetAllAsync(SearchType searchType, bool isIncludeAnnounce)
        {
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("select * from topic where 1=1");
            switch (searchType)
            {
                case SearchType.Visible:
                    builder.AppendToEnd("and is_hide = false");
                    break;
                case SearchType.Hide:
                    builder.AppendToEnd("and is_hide = true");
                    break;
            }
            if (!isIncludeAnnounce)
            {
                builder.AppendToEnd("and is_announce = false");
            }
            builder.AppendToEnd("order by order_num desc;");
            return await this.DbConnection.SelectAsync<Topic>(builder);
        }

        /// <summary>
        /// 获取公告栏
        /// </summary>
        /// <returns></returns>
        public async Task<Topic> GetAnnounce()
        {
            string sql = "SELECT * FROM topic where is_announce = true limit 1;";
            return await this.DbConnection.FirstOrDefaultAsync<Topic>(sql);
        }

        /// <summary>
        /// 设置是否隐藏
        /// </summary>
        public async Task<int> SetHideAsync(Topic topic, bool isHide, DbTransaction transaction = null)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            string sql = "update topic set is_hide = @isHide where id=@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { id = topic.Id, isHide = isHide }, transaction);
        }
    }
}