using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Library.DAL
{
    public class PersonDataRepository : BaseRepository<PersonData>
    {
        public PersonDataRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
