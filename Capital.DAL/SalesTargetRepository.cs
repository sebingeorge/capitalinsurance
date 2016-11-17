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
    public class SalesTargetRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<SalesTarget> GetEmployees()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select  SalesMgId,SalesMgName from SalesManager order by SalesMgName";
                return connection.Query<SalesTarget>(query).ToList();
            }
        }
    }
}
