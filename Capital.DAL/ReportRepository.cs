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
 public  class ReportRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<AgeingSummary> GetAgeingSummaryBasedCommittedDate(string Client="")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"

                      SELECT P.CusId,C.CusName,0 Overdue,sum(P.TotalPremium)TotalPremium,0 Amount1,0 Amount2,0 Amount3,0 Amount4,0 Amount5 INTO #Result FROM PolicyIssue P 
                     inner join Customer C ON P.CusId=c.CusId 
					 where P.PolicyStage=3
                     group by  P.CusId,C.CusName;

                    
                     with A as (
                     select p.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId
                     WHERE CommittedDate <  GETDATE() and pc.paid=0 group by CusId)
                     update R set R.Overdue = A.Amount from A inner join #Result R on R.CusId = A.CusId;
                     
                     with A as (
                     select p.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails PC
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId where pc.paid=1 group by CusId)
                     update R set TotalPremium =(TotalPremium- A.Amount) from A inner join #Result R on R.CusId = A.CusId;
              

                     with A as (
                     select P.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate) <=15 and  DATEDIFF(DAY, GETDATE(), CommittedDate)>0 and pc.paid=0 group by CusId)
                     update R set R.Amount1 = A.Amount from A inner join #Result R on R.CusId = A.CusId;
                    
                     with A as (
                     select P.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate) BETWEEN 15 AND 30 and pc.paid=0  group by CusId)
                     update R set R.Amount2 = A.Amount from A inner join #Result R on R.CusId = A.CusId;
                     
                      with A as (
                     select P.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  BETWEEN 30 AND 60  and pc.paid=0 group by CusId)
                     update R set R.Amount3 = A.Amount from A inner join #Result R on R.CusId = A.CusId;
                     
                     with A as (
                     select P.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId=pc.PolicyId WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  BETWEEN 60 AND 90 and pc.paid=0  group by CusId)
                     update R set R.Amount4 = A.Amount from A inner join #Result R on R.CusId = A.CusId;
                     
                     with A as (
                     select p.CusId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails pc
                     inner join PolicyIssue p on p.PolicyId = pc.PolicyId
                     WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  >90  and pc.paid=0 group by CusId)
                     
                     update R set R.Amount5 = A.Amount from A inner join #Result R on R.CusId = A.CusId;	


				     select * from #Result where  CusName LIKE '%'+@Client+'%' order by CusName";
    




                return connection.Query<AgeingSummary>(query, new { Client = Client }).ToList();
            }
        }

        public List<AgeingSummary> GetAgeingSummaryBasedDetailed(int cuscode)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"

                        select TranPrefix +'/'+TranNumber PolicyNo,TranDate Date,InsPrdName Coverage,CommittedDate,CommittedAmt CommittedAmount,case when paid=1 then 'Yes' else 'No' end as Paid from PolicyIssue p
                        inner join InsuranceProduct ip on p.InsPrdId=ip.InsPrdId
                        inner join PolicyIssueCommittedDetails pc on pc.PolicyId=p.PolicyId
                        where p.CusId=@cuscode ";

                 List<AgeingSummary> list= connection.Query<AgeingSummary>(query, new { cuscode = cuscode }).ToList();
                if (list.Count>0)
                {
                    query = @" select cast(sum(pc.CommittedAmt) as decimal(18,2)) from PolicyIssue p
                                inner join PolicyIssueCommittedDetails pc on pc.PolicyId=p.PolicyId
                                where p.CusId=@cuscode and paid=0";

                    list[0].netamount = connection.Query<Decimal>(query, new { cuscode = cuscode }).First();

                }

                return list;

               
            }
        }
        public List<RenewalSummary> GetPolicyRenewalSummary(string Client = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"

                               ;WITH DateRange AS(
                                               SELECT GETDATE() Months
                                               UNION ALL
                                               SELECT DATEADD(mm, 1, Months)
                                               FROM   DateRange
                                               WHERE Months < DATEADD(mm, 3, GETDATE()))

                                SELECT  Month(Months) AS MonthCode,DateName(m, Months) AS Months,year(Months)Year,0 CountofExpired,0 CountofRenewed into #TEMP FROM DateRange

                                update  #TEMP   set CountofExpired = (select count(*)  from PolicyIssue p where month(p.RenewalDate)=#TEMP.MonthCode and year(p.RenewalDate)=#TEMP.year and p.TranType <>'EndorsePolicy') where MonthCode=#TEMP.MonthCode
                                update  #TEMP   set CountofRenewed = (select count(*)  from PolicyIssue p where month(p.RenewalDate)=#TEMP.MonthCode and year(p.RenewalDate)=#TEMP.year and p.TranType <>'EndorsePolicy' and p.PolicyId =p.OldPolicyId) where MonthCode=#TEMP.MonthCode

                                select *,concat(Months ,' / ', Year)Month from #TEMP";

                return connection.Query<RenewalSummary>(query).ToList();
            }
        }
        public List<PolicyIssue> GetPolicyDetails(int id = 0)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,P.CustContPersonName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    where month(p.RenewalDate)=@id and year(p.RenewalDate)=year(getdate())";
                return connection.Query<PolicyIssue>(query, new { id = id }).ToList();
            }
        }

        public List<PolicyIssue> GetPolicyDetailsComplite()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,P.CustContPersonName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    where year(p.RenewalDate)=year(getdate()) Order by  month(p.RenewalDate)";
                return connection.Query<PolicyIssue>(query).ToList();
            }
        }
        public List<PolicyIssue> GetInsuranceCompanyPayable()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select IC.InsCmpId,InsCmpName,sum(Cd.CommittedAmt)CommittedAmt from [dbo].[PolicyIssue] PI
                                inner join InsuranceCompany IC on IC.InsCmpId=PI.InsCmpId
                                inner join PolicyIssueCommittedDetails CD on cd.PolicyId=pi.PolicyId
                                where paid=1 and InsPaid=0
                                group by IC.InsCmpId,InsCmpName";

                             

                return connection.Query<PolicyIssue>(query).ToList();
            }
        }
        public List<PolicyIssue> GetPolicyDetailsforPayablePopUp(int id = 0)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select Pi.PolicyId,pi.PolicyNo,sum(Cd.CommittedAmt)CommittedAmt from [dbo].[PolicyIssue] PI
                                inner join InsuranceCompany Ic on IC.InsCmpId=PI.InsCmpId
                                inner join PolicyIssueCommittedDetails CD on cd.PolicyId=pi.PolicyId
                                where paid=1 and InsPaid=0 and pi.InsCmpId=@id
                                group by Pi.PolicyId,pi.PolicyNo";
                return connection.Query<PolicyIssue>(query, new { id = id }).ToList();
            }
        }
    }
}
