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

                string query = "select  S.SalesMgName,S.SalesMgId,DsgName from [User]  U INNER JOIN SalesManager S ON S.SalesMgId=U.SalesMgId INNER JOIN Designation D ON S.DsgId=D.DsgId WHERE U.UserId=@Id";
                return connection.Query<DailyActivity>(query, new { Id = Id }).First<DailyActivity>();
            }
        }
        public Result Insert(DailyActivity model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = string.Empty;
                    DateTime TranDate = model.TranDate;
                    int SalesMgId = model.SalesMgId;
                    int CreatedBy = model.CreatedBy;
                    DateTime CreatedDate = DateTime.Now;

                    //string query = @"DELETE FROM SalesTarget WHERE FyId =" + FyId + "";
                    //connection.Execute(query, new { SalesMgCode = SalesMgCode });
                    foreach (var item in model.DailyActivityItems)
                    {
                        sql = @"INSERT INTO DailyActivity
                                (TranDate,SalesMgId,DailyActivityDate,DailyActivityTime,DailyActivityCompany,DailyActivityContactNo,DailyActivityContactPerson,DailyActivityEmail,DailyActivityType,DailyActivityRemarks,CreatedBy,CreatedDate)
                                 VALUES('" + TranDate + "'," + SalesMgId + ",@DailyActivityDate,@DailyActivityTime,@DailyActivityCompany,@DailyActivityContactNo,@DailyActivityContactPerson,@DailyActivityEmail,@DailyActivityType,@DailyActivityRemarks," + CreatedBy + ",'" + CreatedDate + "');  SELECT CAST(SCOPE_IDENTITY() as int);";
                               
                    }
                    int id = connection.Execute(sql, model.DailyActivityItems);
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
        public List<DailyActivityItem> GetDailyActivityDetails(DateTime? From,DateTime? To,int Id = 0,string type="")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select * from DailyActivity where SalesMgId=@Id and TranDate >=CAST(@From AS date) and Trandate <=CAST(@To AS date) and DailyActivityType=case @type when 'all' then DailyActivityType else @type end ";
                return connection.Query<DailyActivityItem>(query, new { Id = Id, From = From, To = To, type = type }).ToList();
            }
        }
   }

   
}
