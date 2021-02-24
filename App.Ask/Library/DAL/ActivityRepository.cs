using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Library.DAL
{
    public class ActivityRepository : BaseRepository<Activity>
    {
        public ActivityRepository(DbConnection connection) : base(connection)
        {

        }
    }
}
