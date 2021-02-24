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
    /// 赞存储库
    /// </summary>
    public class ZanRepository : BaseRepository<Zan>
    {
        public ZanRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// 检查用户是否点赞
        /// </summary>
        public async Task<bool> IsZanAsync(Guid personId, Guid postOrCommentId, ZanType zanType)
        {
            string sql;
            switch (zanType)
            {
                case ZanType.Post:
                    sql = "select count(*) from zan where zan_type =@zanType and person_id=@personId and post_id = @id;";
                    break;
                case ZanType.Comment:
                    sql = "select count(*) from zan where zan_type =@zanType and person_id=@personId and comment_id = @id;";
                    break;
                default:
                    throw new NotSupportedException("不支持默认枚举");
            }
            long count = await this.DbConnection.ScalarAsync<long>(sql, new { personId = personId, id = postOrCommentId, zanType = zanType });
            return count > 0;
        }

        /// <summary>
        /// 检查当前SESSION会话是否点赞
        /// </summary>
        public async Task<bool> IsZanAsync(string sessionId, Guid postOrCommentId, ZanType zanType)
        {
            string sql;
            switch (zanType)
            {
                case ZanType.Post:
                    sql = "select count(*) from zan where zan_type =@zanType and session_id=@sessionId and post_id = @id;";
                    break;
                case ZanType.Comment:
                    sql = "select count(*) from zan where zan_type =@zanType and session_id=@sessionId and comment_id = @id;";
                    break;
                default:
                    throw new NotSupportedException("不支持默认枚举");
            }
            long count = await this.DbConnection.ScalarAsync<long>(sql, new { sessionId = sessionId, id = postOrCommentId, zanType = zanType });
            return count > 0;
        }
    }
}