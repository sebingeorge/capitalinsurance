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

                     SELECT P.PolicyId,C.CusName,P.TranType,P.TotalPremium,0 Amount1,0 Amount2,0 Amount3,0 Amount4,0 Amount5 INTO #Result FROM PolicyIssue P inner join Customer C ON C.CusId=P.CusId where P.PolicyNo IS NOT NULL;
                     
                     with A as (
                     select PolicyId, sum(ChequeAmt)Amount from PolicyIssueChequeReceived  group by PolicyId)
                     update R set TotalPremium =(TotalPremium- A.Amount) from A inner join #Result R on R.PolicyId = A.PolicyId;

                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate) <=15  group by PolicyId)
                     update R set R.Amount1 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                    
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate) BETWEEN 15 AND 30  group by PolicyId)
                     update R set R.Amount2 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                     
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  BETWEEN 30 AND 60  group by PolicyId)
                     update R set R.Amount3 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                     
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  BETWEEN 60 AND 90  group by PolicyId)
                     update R set R.Amount4 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                     
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  DATEDIFF(DAY, GETDATE(), CommittedDate)  >90  group by PolicyId)
                     
                     update R set R.Amount5 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;	


				     select * from #Result where  CusName LIKE '%'+@Client+'%'";





                return connection.Query<AgeingSummary>(query, new { Client = Client }).ToList();
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
                                    month(p.RenewalDate)=@id";


                return connection.Query<PolicyIssue>(query, new { id = id }).ToList();
            }
        }
    }
}
