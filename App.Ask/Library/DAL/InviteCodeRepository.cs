using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 邀请码存储库
    /// </summary>
    public class InviteCodeRepository : BaseRepository<InviteCode>
    {
        public InviteCodeRepository(DbConnection connection) : base(connection) { }

        /// <summary>
        /// 检查是否存在邀请码
        /// </summary>
        public async Task<bool> IsExistCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            string sql = "SELECT count(*) FROM invite_code WHERE upper(code) = @code LIMIT 1;";
            long result = await this.DbConnection.ScalarAsync<long>(sql, new { code = code.ToUpper() });
            return result > 0;
        }

        /// <summary>
        /// 删除邀请码
        /// </summary>
        public async Task<int> DeleteAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            string sql = "delete from invite_code where upper(code) = @code;";
            return await this.DbConnection.ExecuteNonQueryAsync(sql, new { code = code.Trim().ToUpper() });
        }

        /// <summary>
        /// 创建新的邀请码
        /// </summary>
        public async Task<InviteCode> CreateNewCodeAsync(Person person, DbTransaction transaction)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            string code = RandomHelper.GetIntNumber(100000, 999999).ToString();
            InviteCode inviteCode = new InviteCode
            {
                Id = GuidHelper.CreateSequential(),
                PersonId = person.Id,
                Code = code
            };
            await this.InsertAsync(inviteCode, transaction);
            return inviteCode;
        }
    }
}
