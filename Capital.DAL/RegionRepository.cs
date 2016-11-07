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
    public class RegionRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<Region> GetRegions()
        {
             using (IDbConnection connection = OpenConnection(dataConnection))
             {
                 return connection.Query<Region>("select RegionId, RegionName from Region order by RegionName").ToList();
             }
        }
    }
}
