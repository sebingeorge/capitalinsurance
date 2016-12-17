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

        public List<SalesAchievement> GetSalesAchievementReportDetails(int? FyId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select S.SalesMgId,SM.SalesMgName,SM.SalesMgCode,F.FyName,S.FyId,S.Quarer1 Target1,S.Quarer2 Target2,S.Quarer3 Target3,S.Quarer4 Target4,0 Achvd1,0 Achvd2,0 Achvd3,0 Achvd4,0 AchvdPerc1,0 AchvdPerc2,0 AchvdPerc3,0 AchvdPerc4 INTO #RESULT from SalesTarget S 
                               INNER JOIN SalesManager SM on S.SalesMgId=SM.SalesMgId
                               INNER JOIN FinancialYear F ON F.FyId=S.FyId;
        
                               with A as (
                               select P.SalesMgId, sum(P.TotalPremium)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) in (1,2,3) and year(P.TranDate)=R.FyName group by P.SalesMgId)
                               Update R set Achvd1 = A.Amount from A inner join #Result R on R.SalesMgId = A.SalesMgId;
                               
                               with A as (
                               select P.SalesMgId, sum(TotalPremium)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(TranDate)in(4,5,6) and year(P.TranDate)=R.FyName group by P.SalesMgId)
                               Update R set Achvd2 = A.Amount from A inner join #Result R on R.SalesMgId = A.SalesMgId;
                               
                               with A as (
                               select P.SalesMgId, sum(TotalPremium)Amount from PolicyIssue  P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(TranDate)in(7,8,9) and year(P.TranDate)=R.FyName  group by P.SalesMgId)
                               Update R set Achvd3 = A.Amount from A inner join #Result R on R.SalesMgId = A.SalesMgId;
                               
                               with A as (
                               select P.SalesMgId, sum(TotalPremium)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(TranDate)in(10,11,12) and year(P.TranDate)=R.FyName  group by P.SalesMgId)
                               Update R set Achvd4 = A.Amount from A inner join #Result R on R.SalesMgId = A.SalesMgId;

                               Update R set AchvdPerc1 = (Achvd1/Target1)*100 from #Result R where Achvd1>0;
                               Update R set AchvdPerc2 = (Achvd2/Target2)*100 from #Result R where Achvd2>0;
                               Update R set AchvdPerc3 = (Achvd3/Target3)*100 from #Result R where Achvd3>0;
                               Update R set AchvdPerc4 = (Achvd4/Target4)*100 from #Result R where Achvd4>0;
                               SELECT * FROM #RESULT  WHERE FyId=@FyId";

                var objSalesTarget = connection.Query<SalesAchievement>(sql, new { FyId = FyId }).ToList<SalesAchievement>();
                return objSalesTarget;
            }
        }
    }
}
