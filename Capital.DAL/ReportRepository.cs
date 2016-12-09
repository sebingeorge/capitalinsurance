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
    }
}
