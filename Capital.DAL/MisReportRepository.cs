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
							where DATEPART(month,TranDate)=DATEPART(month,GETDATE())
							group by InsPrdName order by TotalAmount desc";

                return connection.Query<MonthlyAcheivementcoveragewise>(sql);
            }
        }


        public IEnumerable<MonthlySales> GetmonthlySales()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select  top 12 CONVERT(CHAR(4), CusInvoiceDate, 100) + CONVERT(CHAR(4),  CusInvoiceDate, 120) Monthly,sum(TotalAmount) TotalAmount from [CustomerInvoice]
                               group by CusInvoiceDate order by CusInvoiceDate desc";

                return connection.Query<MonthlySales>(sql);
            }
        }



        public IEnumerable<EmployeeAchievementVsTraget> GetEmployeeTargetAchivement()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select sm.SalesMgName slmgname ,sm.SalesMgId slmgcode,sum(TotalPremium) achivement,0 target  into #Temp from  PolicyIssue Pi
                               
                                inner join SalesManager SM on SM.SalesMgId= Pi.SalesMgId
                                group by sm.SalesMgName,sm.SalesMgId;

                            with A as(select SalesMgId SalesMg,sum(Total) total from SalesTarget St inner join FinancialYear FY on FyName=  DATEPART(Year, GETDATE())  where St.FyId=fy.FyId group by SalesMgId )

                            update #Temp set target=A.total from A where #Temp.slmgcode=a.SalesMg;

							select * from #Temp";

                return connection.Query<EmployeeAchievementVsTraget>(sql);
            }
        }

    }
}