using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Info;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Text;
using SiHan.Libs.Utils.Time;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace App.Ask.Library.DAL
{
    public class PersonRepository : BaseRepository<Person>
    {
        public PersonRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// 同时检查账号、昵称是否存在
        /// </summary>
        public async Task<bool> IsExistNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(name);
            }
            name = name.Trim().ToUpper();
            string sql = "select count(*) from person where upper(nick_name) =@p or upper(account_name) =@p;";
            long count = await this.DbConnection.ScalarAsync<long>(sql, new { p = name });
            return count > 0;
        }

        /// <summary>
        /// 通过账号获取用户(该账户可能已被软删除)
        /// </summary>
        public async Task<Person> GetByAccountNameAsync(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentNullException(nameof(accountName));
            }
            string sql = "SELECT * FROM person WHERE upper(account_name) = @accountName LIMIT 1; ";
            return await this.DbConnection.FirstOrDefaultAsync<Person>(sql, new { accountName = accountName.Trim().ToUpper() });
        }

        public override async Task<int> DeleteAsync(Person entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await this.DeleteByIdAsync(entity.Id, transaction);
        }

        /// <summary>
        /// 软删除用户
        /// </summary>
        public override async Task<int> DeleteByIdAsync(Guid id, DbTransaction transaction = null)
        {
            return await this.DbConnection.ExecuteNonQueryAsync("UPDATE person SET is_delete = TRUE WHERE id=@id;", new { id = id }, transaction);
        }

        /// <summary>
        /// 恢复被删除的用户
        /// </summary>
        public async Task<int> RestoreRemoveByIdAsync(Guid id, DbTransaction transaction = null)
        {
            return await this.DbConnection.ExecuteNonQueryAsync("UPDATE person SET is_delete = FALSE WHERE id=@id;", new { id = id }, transaction);
        }

        /// <summary>
        /// 禁言用户
        /// </summary>
        public async Task<int> MuteByIdAsync(Guid id, DbTransaction transaction = null)
        {
            return await this.DbConnection.ExecuteNonQueryAsync("UPDATE person SET is_mute = TRUE WHERE id=@id;", new { id = id }, transaction);
        }

        /// <summary>
        /// 恢复被禁言用户
        /// </summary>
        public async Task<int> RestoreMuteByIdAsync(Guid id, DbTransaction transaction = null)
        {
            return await this.DbConnection.ExecuteNonQueryAsync("UPDATE person SET is_mute = FALSE WHERE id=@id;", new { id = id }, transaction);
        }

        public override Task<int> DeleteMoreAsync(List<Person> entities, DbTransaction transaction)
        {
            throw new InvalidOperationException("Bulk deletion of person is not allowed");
        }

        /// <summary>
        /// 获取最近一周的注册数量
        /// </summary>
        public async Task<List<PersonRegisterInfo>> GetWeekRegisterAsync()
        {
            DateTime startTime = DateTimeHelper.GetStartDateTime(DateTime.Now.AddDays(-7));
            DateTime endTime = DateTimeHelper.GetEndDateTime(DateTime.Now);
            string sql = "select date(create_time) as create_date,count(id) as create_count from person where create_time >= @startTime and create_time <=@endTime group by create_date;";
            return await this.DbConnection.SelectAsync<PersonRegisterInfo>(sql, new { startTime = startTime, endTime = endTime });
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        public async Task<int> ModifyRole(Guid personId, Role role, DbTransaction trans = null)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            string sql = "update person set role_id = @roleId where id = @id";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { id = personId, roleId = role.Id }, trans);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        public async Task<int> ModifyNickNameAsync(Guid personId, string newNickName)
        {
            if (string.IsNullOrWhiteSpace(newNickName))
            {
                throw new ArgumentNullException(nameof(newNickName));
            }
            string sql = "update person set nick_name = @p where id = @id";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { p = newNickName.Trim(), id = personId });
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        public async Task<int> ModifyAvatarAsync(Guid personId, string avatar)
        {
            if (string.IsNullOrWhiteSpace(avatar))
            {
                throw new ArgumentNullException(nameof(avatar));
            }
            string sql = "update person set avatar = @avatar where id=@id";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { avatar = avatar.Trim(), id = personId });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public async Task<int> ModifyPasswordAsync(Guid personId, string pwdHash, DbTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(pwdHash))
            {
                throw new ArgumentNullException(nameof(pwdHash));
            }
            string sql = "update person set password = @p where id=@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { p = pwdHash, id = personId }, trans);
        }

        /// <summary>
        /// 刷新用户的更新时间
        /// </summary>
        public async Task<int> UpdateLastTimeAsync(Guid personId, DbTransaction trans)
        {
            string sql = "update person set last_updated = @p where id=@id;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { p = DateTime.Now, id = personId }, trans);
        }
    }
}