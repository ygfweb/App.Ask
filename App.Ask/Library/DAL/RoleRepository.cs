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
    /// 角色存储库
    /// </summary>
    public class RoleRepository
    {
        private DbConnection Connection { get; }

        public RoleRepository(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await this.Connection.SingleByIdAsync<Role>(id);
        }



        public async Task<Role> GetSingleAsync(RoleType roleType)
        {
            List<Role> roles = await this.Connection.SelectAsync<Role>("select * from role where role_type = @roleType;", new { roleType = roleType });
            if (roles == null || roles.Count == 0)
            {
                return null;
            }
            else
            {
                return roles[0];
            }
        }
    }
}
