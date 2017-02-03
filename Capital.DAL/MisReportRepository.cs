using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capital.Domain;
using Dapper;
using System.Data;
namespace Capital.DAL
{
    public class MisReportRepository : BaseRepository
    {

        static string dataConnection = GetConnectionString("CibConnection");
        public IEnumerable<MonthlyAcheivementcoveragewise> GetmonthlycoverageAchivement()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select InsPrdName coverage,sum(CommissionAmount) TotalAmount from PolicyIssue p
							inner join InsuranceProduct ip on ip.InsPrdId=p.InsPrdId
							where DATEPART(month,TranDate)=DATEPART(month,GETDATE()) and DATEPART(YEAR,TranDate)=DATEPART(YEAR,GETDATE())
							group by InsPrdName order by TotalAmount desc";

                return connection.Query<MonthlyAcheivementcoveragewise>(sql);
            }
        }


        public IEnumerable<MonthlySales> GetmonthlySales()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @" select CONVERT(CHAR(4), TranDate, 100) + CONVERT(CHAR(4),  TranDate, 120) Monthly,sum(PremiumAmount) TotalAmount,DATEPART(MM,TranDate) MONTHC,DATEPART(YYYY,TranDate) YRARC INTO  #TEMPSALES from PolicyIssue WHERE TranDate>=DATEADD(month, -6, GETDATE()) and trandate<=GETDATE()
                               group by TRaNDATE

							   SELECT   Monthly,SUM(TotalAmount) TotalAmount FROM #TEMPSALES GROUP BY Monthly, YRARC,MONTHC ORDER BY YRARC,MONTHC ASC ";

                return connection.Query<MonthlySales>(sql);
            }
        }



        public IEnumerable<EmployeeAchievementVsTraget> GetEmployeeTargetAchivement()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select sm.SalesMgName slmgname ,sm.SalesMgId slmgcode,sum(TotalPremium) achivement,0 target  into #Temp from  PolicyIssue Pi
                               
                                inner join SalesManager SM on SM.SalesMgId= Pi.SalesMgId
								where DATEPART(Year,pi.TranDate)=  DATEPART(Year, GETDATE()) 
                                group by sm.SalesMgName,sm.SalesMgId;

                            with A as(select SalesMgId SalesMg,sum(Total) total from SalesTarget St inner join FinancialYear FY on FyName=  DATEPART(Year, GETDATE())  where St.FyId=fy.FyId group by SalesMgId )

                            update #Temp set target=A.total from A where #Temp.slmgcode=a.SalesMg;

							select * from #Temp";

                return connection.Query<EmployeeAchievementVsTraget>(sql);
            }
        }


        public IEnumerable<CoverageVsSales> GetCoverageVsSales()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select ip.InsPrdName Coverage,sum(PremiumAmount) TotalAmount from PolicyIssue p
							inner join InsuranceProduct ip on ip.InsPrdId=p.InsPrdId
							 where DATEPART(YEAR,TranDate)= DATEPART(YEAR,GETDATE())
							 group by ip.InsPrdName";

                return connection.Query<CoverageVsSales>(sql);
            }
        }

    }
}