using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;

namespace App.Ask.Library.DAL
{
    public class PostDataRepository : BaseRepository<PostData>
    {
        public PostDataRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }
        public async Task<PostData> GetPostDataAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            string sql = "select * from post_data where post_id = @postId  limit  1";
            return await this.DbConnection.FirstOrDefaultAsync<PostData>(sql, new { postId = post.Id });
        }
    }
}
