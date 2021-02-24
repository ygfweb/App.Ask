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
    public class PersonViewRepository
    {
        private readonly DbConnection connection;

        public PersonViewRepository(DbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<PersonView> GetByIdAsync(Guid id)
        {
            string sql = "select * from person_view where id=@id limit 1;";
            return await this.connection.FirstOrDefaultAsync<PersonView>(sql, new { id = id });
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        public async Task<PagingResult<PersonView>> SearchAsync(string text, SearchType searchType = SearchType.Visible, RoleFilterType roleFilter = RoleFilterType.All, int currentPage = 1, int pageSize = 10)
        {
            SqlBuilder builder = new SqlBuilder();
            builder.AppendToEnd("from person_view where");
            builder.AppendToEnd("(account_name like @p1 or nick_name like @p1)").AddParameter("p1", "%" + text + "%");
            switch (searchType)
            {
                case SearchType.Visible:
                    builder.AppendToEnd(" and is_delete is false");
                    break;
                case SearchType.Hide:
                    builder.AppendToEnd(" and is_delete is true");
                    break;
            }
            switch (roleFilter)
            {
                case RoleFilterType.Admin:
                    builder.AppendToEnd("and role_type = @roleType").AddParameter("roleType", RoleType.Admin);
                    break;
                case RoleFilterType.Master:
                    builder.AppendToEnd("and role_type = @roleType").AddParameter("roleType", RoleType.Master);
                    break;
                case RoleFilterType.User:
                    builder.AppendToEnd("and role_type = @roleType").AddParameter("roleType", RoleType.User);
                    break;
            }
            builder.AppendToEnd("and role_type != @SuperAdminType").AddParameter("SuperAdminType", RoleType.SuperAdmin);
            SqlBuilder countBuiler = builder.Clone() as SqlBuilder;
            countBuiler.InsertToStart("select count(*)");
            long count = await this.connection.ScalarAsync<long>(countBuiler);
            builder.AppendToEnd("order by create_time desc");
            builder.SetPaging(currentPage, pageSize);
            builder.InsertToStart("select *");
            List<PersonView> personViews = await this.connection.SelectAsync<PersonView>(builder);
            return new PagingResult<PersonView>(currentPage, pageSize, count, personViews);
        }
    }
}
