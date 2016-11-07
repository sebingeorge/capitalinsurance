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
    public class SalesManagerRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<SalesManager> GetSalesManagers()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<SalesManager>("select SalesMgId, SalesMgName from SalesManager").ToList();
            }
        }
    }
}
