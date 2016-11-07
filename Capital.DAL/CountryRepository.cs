using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capital.Domain;
using System.Data;
using Dapper;

namespace Capital.DAL
{
    public class CountryRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<Country> GetCountry()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Country>("select CountryId, CountryName from Country").ToList();
            }
        }
    }
}
