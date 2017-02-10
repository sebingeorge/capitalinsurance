﻿using System;
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
//                string sql = @"select FyId,ST.SalesMgId,SalesMgName,Quarer1,Quarer2,Quarer3,Quarer4,Total from SalesTarget ST
//                               left join SalesManager S on S.SalesMgId=ST.SalesMgId where FyId=" + FyId + "  order by SalesMgName";
                string sql = @"select FyId,S.SalesMgId,SalesMgName,Quarer1,Quarer2,Quarer3,Quarer4,Total from SalesManager S   left join SalesTarget ST
                               on S.SalesMgId=ST.SalesMgId where DsgId in (9,10,11,2) and FyId=" + FyId + "  order by SalesMgName";
                var objSalesTarget = connection.Query<SalesTargetItem>(sql).ToList<SalesTargetItem>();
                return objSalesTarget;
            }
        }
        public List<SalesTargetItem> GetEmployees()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from SalesManager where DsgId in (9,10,11,2) order by SalesMgName";
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

        public List<SalesAchievement> GetSalesIncentiveReportDetails(int Id, int? FyId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select SM.SalesMgId,SM.SalesMgCode,SM.SalesMgName,IncentivePerc,(3*MonthlySalary)Benchmark,0 janComm,0 janInctve,0 febComm,0 febInctve,
                0 marComm,0 marInctve,0 aplComm,0 aplInctve ,0 mayComm,0 mayInctve,0 juneComm,0 juneInctve,
                0 julyComm,0 julyInctve,0 augComm,0 augInctve ,0 sepComm,0 sepInctve,0 octComm,0 octInctve,0 novComm,0 novInctve 
                ,0 decComm,0 decInctve  INTO #RESULT
                from SalesManager SM 

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (1)group by P.SalesMgId)
                Update R set janComm = A.Amount,janInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (2)group by P.SalesMgId)
                Update R set febComm = A.Amount,febInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (3)group by P.SalesMgId)
                Update R set marComm = A.Amount,marInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (4)group by P.SalesMgId)
                Update R set aplComm = A.Amount,aplInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (5)group by P.SalesMgId)
                Update R set mayComm = A.Amount,mayInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (6)group by P.SalesMgId)
                Update R set juneComm = A.Amount,juneInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (7)group by P.SalesMgId)
                Update R set julyComm = A.Amount,julyInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (8)group by P.SalesMgId)
                Update R set augComm = A.Amount,augInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (9)group by P.SalesMgId)
                Update R set sepComm = A.Amount,sepInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (10)group by P.SalesMgId)
                Update R set octComm = A.Amount,octInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (11)group by P.SalesMgId)
                Update R set novComm = A.Amount,novInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                with A as (
                select P.SalesMgId, sum(P.TotalCommission)Amount from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId inner join SalesManager S ON S.SalesMgId=P.SalesMgId where month(P.TranDate) in (12)group by P.SalesMgId)
                Update R set decComm = A.Amount,decInctve=(A.Amount-R.Benchmark)*(R.IncentivePerc/100) from A inner join #Result R on R.SalesMgId = A.SalesMgId ;

                SELECT * FROM #Result";

                var objSalesTarget = connection.Query<SalesAchievement>(sql, new {Id=Id, FyId = FyId }).ToList<SalesAchievement>();
                return objSalesTarget;
            }
        }
        public List<SalesAchievement> GetTotalSalesTargetReport(int Id,int? FyId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select SalesMgId into #TEMP from [User]  U  WHERE U.UserId=@Id 
                               union all
                               select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )
                               union all
                               select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id ))
                               union all
                               select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )))

                               select S.SalesMgId,SM.SalesMgName,S.FyId,F.FyName,S.Total,0 TotalAcvd,S.Quarer1 Target1,S.Quarer2 Target2,
                               S.Quarer3 Target3,S.Quarer4 Target4,0 Jan,0 Feb,0 Mar,0 Apl,0 May,0 Jun,0 July,0 Aug,0 Sep,0 Oct,0 Nov,0 Dec,0 Q1Shortfall,
                               0 Q2Shortfall,0 Q3Shortfall,0 Q4Shortfall,0 Q1Excess,0 Q2Excess,0 Q3Excess,0 Q4Excess INTO #RESULT from SalesTarget S
                               INNER JOIN SalesManager SM on S.SalesMgId=SM.SalesMgId
                               INNER JOIN FinancialYear F ON F.FyId=S.FyId;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)TotalAcvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where  year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set TotalAcvd = A.TotalAcvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)JanAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 1 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Jan = A.JanAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)FebAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 2 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Feb = A.FebAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)MarAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 3 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Mar = A.MarAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)AplAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 4 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Apl = A.AplAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)MayAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 5 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set May = A.MayAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)JunAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 6 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Jun = A.JunAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)JulyAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 7 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set July = A.JulyAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)AugAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 8 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Aug = A.AugAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)SepAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 9 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Sep = A.SepAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)OctAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 10 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Oct = A.OctAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select  year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)NovAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 11 and year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Nov = A.NovAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;

                               with A as (
                               select  year(P.TranDate)year,P.SalesMgId, sum(P.TotalCommission)DecAchvd from PolicyIssue P INNER JOIN  #Result R on R.SalesMgId= P.SalesMgId where month(P.TranDate) = 12 and  year(P.TranDate)=R.FyName group by P.SalesMgId,year(P.TranDate))
                               Update R set Dec = A.DecAchvd from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.year=R.FyName;
                              
                               with A as (
                               select  FyName,SalesMgId, Target1-(Jan + Feb + Mar)Q1Shortfall ,(Jan + Feb + Mar)-Target1 Q1Excess from #Result  )
                               Update R set Q1Shortfall = A.Q1Shortfall,Q1Excess=A.Q1Excess from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               with A as (
                               select  FyName,SalesMgId, Target2+(Q1Shortfall/3)Target2 ,Target3+(Q1Shortfall/3)Target3,Target4+(Q1Shortfall/3)Target4 from #Result where Q1Shortfall>0 )
                               Update R set Target2 = A.Target2,Target3 = A.Target3,Target4 = A.Target4 from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               
                               with A as (
                               select  FyName,SalesMgId,  Target2-(Apl + May + Jun)Q2Shortfall  ,(Apl + May + Jun)-Target2 Q2Excess from #Result  )
                               Update R set Q2Shortfall = A.Q2Shortfall,Q2Excess=A.Q2Excess  from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               with A as (
                               select  FyName,SalesMgId, (Target3-(Q2Excess/2)+(Q2Shortfall/2))Target3 ,Target4-(Q2Excess/2)Target4 from #Result where Q2Shortfall>0 and Q2Excess>0  )
                               Update R set Target3 = A.Target3,Target4 = A.Target4 from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               with A as (
                               select  FyName,SalesMgId, Target3-(july + Aug + Sep)Q3Shortfall  ,(july + Aug + Sep)-Target3 Q3Excess from #Result  )
                               Update R set Q3Shortfall = A.Q3Shortfall,Q3Excess=A.Q3Excess from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               with A as (
                               select  FyName,SalesMgId, ((Target4)+(Q3Shortfall)-(Q3Excess))Target4  from #Result where Q3Shortfall>0 and Q3Excess>0 )
                               Update R set Target4 = A.Target4 from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               
                               with A as (
                               select  FyName,SalesMgId, Target4-(Oct + Nov + Dec) Q4Shortfall  ,(Oct + Nov + Dec)-Target4 Q4Excess from #Result  )
                               Update R set Q4Shortfall = A.Q4Shortfall,Q4Excess=A.Q4Excess from A inner join #Result R on R.SalesMgId = A.SalesMgId and A.FyName=R.FyName;
                               

                               SELECT  SalesMgName,Total TotalTarget,TotalAcvd, CASE WHEN (TotalAcvd-Total)<= 0 THEN 0 ELSE (TotalAcvd-Total) END AS YExcess,CASE WHEN (Total-TotalAcvd)<= 0 THEN 0 ELSE (Total-TotalAcvd) END AS YShortFall,Target1,Target2,Target3,Target4,Jan,Feb,Mar,Apl,May,Jun,July,Aug,Sep,Oct,Nov,Dec,
                               CASE WHEN Q1Shortfall<= 0 THEN 0 ELSE Q1Shortfall END AS Q1Shortfall,
                               CASE WHEN Q2Shortfall<= 0 THEN 0 ELSE Q2Shortfall END AS Q2Shortfall,
                               CASE WHEN Q3Shortfall<= 0 THEN 0 ELSE Q3Shortfall END AS Q3Shortfall,
                               CASE WHEN Q4Shortfall<= 0 THEN 0 ELSE Q4Shortfall END AS Q4Shortfall,
                               CASE WHEN Q1Excess<= 0 THEN 0 ELSE Q1Excess END AS Q1Excess,
                               CASE WHEN Q2Excess<= 0 THEN 0 ELSE Q2Excess END AS Q2Excess,
                               CASE WHEN Q3Excess<= 0 THEN 0 ELSE Q3Excess END AS Q3Excess,
                               CASE WHEN Q4Excess <= 0 THEN 0 ELSE Q4Excess END AS Q4Excess FROM #RESULT  WHERE  FyId=@FyId and isnull(SalesMgId,0) IN (SELECT SalesMgId FROM #TEMP)";

                var objSalesTarget = connection.Query<SalesAchievement>(sql, new {Id=Id,FyId = FyId }).ToList<SalesAchievement>();
                return objSalesTarget;
            }
        }
    }
}
