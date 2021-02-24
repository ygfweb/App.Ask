using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Library.Extensions
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// 递增值
        /// </summary>
        public static async Task<int> IncreaseAsync(this DbConnection connection, string className, string propertyName, string whereId, Guid id, DbTransaction transaction)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException(nameof(className));
            }
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (string.IsNullOrWhiteSpace(whereId))
            {
                throw new ArgumentNullException(nameof(whereId));
            }
            className = StringHelper.PascalCaseToUnderscores(className);
            propertyName = StringHelper.PascalCaseToUnderscores(propertyName);
            whereId = StringHelper.PascalCaseToUnderscores(whereId);
            string sql = $"update {className} set {propertyName} = {propertyName} + 1 where {whereId} = @id";
            return await connection.ExecuteNonQueryAsync(sql, new { id = id });
        }

        /// <summary>
        /// 递减值
        /// </summary>
        public static async Task<int> DecreaseAsync(this DbConnection connection, string className, string propertyName, string whereId, Guid id, DbTransaction transaction)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException(nameof(className));
            }
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (string.IsNullOrWhiteSpace(whereId))
            {
                throw new ArgumentNullException(nameof(whereId));
            }
            className = StringHelper.PascalCaseToUnderscores(className);
            propertyName = StringHelper.PascalCaseToUnderscores(propertyName);
            whereId = StringHelper.PascalCaseToUnderscores(whereId);
            string sql = $"update {className} set {propertyName} = {propertyName} - 1 where {whereId} = @id";
            return await connection.ExecuteNonQueryAsync(sql, new { id = id });
        }
    }
}

