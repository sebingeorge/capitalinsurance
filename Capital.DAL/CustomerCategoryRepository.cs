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
    public class CustomerCategoryRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<CustomerCategory> GetCustomerCategory()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<CustomerCategory>("select CusCatId, CusCategory from CustomerCategory").ToList();
            }
        }
    }
}
