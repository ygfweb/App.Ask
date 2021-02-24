using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 活动视图存储库
    /// </summary>
    public class ActivityViewRepository
    {
        private readonly DbConnection connection;

        public ActivityViewRepository(DbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// 获取用户的活动（分页结果）
        /// </summary>
        public async Task<PagingResult<ActivityView>> GetPagingResultAsync(Person person, int currentPage = 1, int pageSize = 10)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from activity_view where person_id = @personId").AddParameter("personId", person.Id);
            SqlBuilder countBuilder = builder.Clone() as SqlBuilder;
            countBuilder.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countBuilder);
            builder.AppendToEnd("order by do_time desc");
            builder.SetPaging(currentPage, pageSize);
            builder.InsertToStart("select *");
            List<ActivityView> views = await this.connection.SelectAsync<ActivityView>(builder);
            return new PagingResult<ActivityView>(currentPage, pageSize, count, views);
        }
    }
}
