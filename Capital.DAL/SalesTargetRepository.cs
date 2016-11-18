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


        public List<SalesTargetItem> GetEmployees(int? FyId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select FyId,ST.SalesMgId,SalesMgName,Quarer1,Quarer2,Quarer3,Quarer4,Total from SalesTarget ST
                               left join SalesManager S on S.SalesMgId=ST.SalesMgId where FyId=" + FyId + "  order by SalesMgName";

                var objSalesTarget = connection.Query<SalesTargetItem>(sql).ToList<SalesTargetItem>();
                return objSalesTarget;
            }
        }
        public List<SalesTargetItem> GetEmployees()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from SalesManager order by SalesMgName";
                var objSalesTarget = connection.Query<SalesTargetItem>(sql).ToList<SalesTargetItem>();
                return objSalesTarget;
            }
        }
        public Result Insert(SalesTarget model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                   string sql=string.Empty;
                   int FyId = model.FyId;
                   string query = @"DELETE FROM SalesTarget WHERE FyId =" + FyId + "";
                   connection.Execute(query, new { FyId = FyId });
                   foreach (var item in model.SalesTargetItems)
                   {
                        sql = @"INSERT INTO SalesTarget
                                (FyId,SalesMgId,Quarer1,Quarer2,Quarer3,Quarer4,Total)
                                 VALUES(" + FyId + ",@SalesMgId,@Quarer1,@Quarer2,@Quarer3,@Quarer4,@Total);SELECT CAST(SCOPE_IDENTITY() as int);";
                    }
                   int id = connection.Execute(sql, model.SalesTargetItems);
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
    }
}
