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
 public  class MonthlyReportRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<AgeingSummary> MonthlyPaymentCommittment(string Client="")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"
declare @LastDay_CurrentMonth datetime;
declare @LastDay_NextMonth datetime;
declare @FirstDay_NextMonth datetime;
declare @LastDay_Next2Month datetime;
declare @FirstDay_Next2Month datetime;
select @LastDay_CurrentMonth = (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)))

select @LastDay_NextMonth =(SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+2,0)))
select @FirstDay_NextMonth =(SELECT DATEADD(s,1,@LastDay_CurrentMonth))
select @FirstDay_Next2Month =(SELECT DATEADD(s,1,@LastDay_NextMonth))
select @LastDay_Next2Month = (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+3,0)))



 SELECT P.PolicyId,C.CusName,P.TranType,P.TotalPremium,0 Amount1,0 Amount2,0 Amount3 INTO #Result FROM PolicyIssue P inner join Customer C ON C.CusId=P.CusId where P.PolicyNo IS NOT NULL;
                     
                     with A as (
                     select PolicyId, sum(ChequeAmt)Amount from PolicyIssueChequeReceived  group by PolicyId)
                     update R set TotalPremium =(TotalPremium- A.Amount) from A inner join #Result R on R.PolicyId = A.PolicyId;

                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE   CommittedDate <= @LastDay_CurrentMonth   group by PolicyId)
                     update R set R.Amount1 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                    
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE   CommittedDate BETWEEN @FirstDay_NextMonth AND @LastDay_NextMonth  group by PolicyId)
                     update R set R.Amount2 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;
                     
                     with A as (
                     select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE  CommittedDate BETWEEN @FirstDay_Next2Month AND @LastDay_Next2Month  group by PolicyId)
                     update R set R.Amount3 = A.Amount from A inner join #Result R on R.PolicyId = A.PolicyId;


				     select * from #Result ;
                   

";





                return connection.Query<AgeingSummary>(query, new { Client = Client }).ToList();
            }
        }
    }
}
