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
    public class DailyActivityRepository : BaseRepository
    {

        static string dataConnection = GetConnectionString("CibConnection");

        public DailyActivity DAEmployeeDetails(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = "select  S.SalesMgName,DsgName from [User]  U INNER JOIN SalesManager S ON S.SalesMgId=U.SalesMgId INNER JOIN Designation D ON S.DsgId=D.DsgId WHERE U.UserId=@Id";
                return connection.Query<DailyActivity>(query, new { Id = Id }).First<DailyActivity>();
            }
        }
   }

   
}
