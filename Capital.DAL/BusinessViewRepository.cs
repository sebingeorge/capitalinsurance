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
    public class BusinessViewRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<PolicyIssue> GetBusinessViewDetails(int Id,string userRolename, string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = "";
                if (userRolename == "Administrator")
                {

                     query = @" select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,convert(char(3), TranDate, 0)Month,year(TranDate)Year,day(TranDate)Day,
                            C.CusName,C.Address1 CusAddress,C.EmailId,C.OfficeNo,C.MobileNo,
                            (C.EmployeeNo + ISNULL(P.AdditionEmpNo,0) - ISNULL(P.DeletionEmpNo,0))EmployeeNo,P.CustContPersonName,P.InsuredName,P.CustContDesignation,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,
                            (Select RenewalDate from PolicyIssue A where TranType='NewPolicy' AND A.PolicyId=P.PolicyId )ExpiryDate,
                            (select A.RenewalDate from PolicyIssue A  where P.TranType='RenewPolicy' and P.PolicyId=A.PolicyId)RenewalDate,
                            P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionPerc,P.CommissionAmount, S.SalesMgName,S.SalesMgCode,S.QuatarContactNo,S.OfficeEmail,P.PolicyNo,P.PolicyFee,P.PaymentTo,P.PolicySubDate,P.TranType,P.EndorcementDate,
                            (select A.PolicyNo from PolicyIssue A  where P.TranType='EndorsePolicy' and P.PolicyId=A.PolicyId)EndorcementNo,P.AdditionEmpNo,
                            CASE WHEN (P.DeletionEmpNo IS NOT NULL) and (P.DeletionEmpNo IS NOT NULL) THEN 'ADD/DEL'
                            WHEN (P.AdditionEmpNo IS NOT NULL) THEN 'ADDITION'
                            WHEN (P.DeletionEmpNo IS NOT NULL) THEN 'DELETION'
                            WHEN (P.DeletionEmpNo IS  NULL ) or (P.AdditionEmpNo IS  NULL )
                            THEN '-' END AS EndType,
                            P.DeletionEmpNo,DATEDIFF(dd,P.RenewalDate,GETDATE ()) Aging,ICActualDate,0 cibpaid, 0 BalanceRecivable,0 InsCompPaid INTO #TEMP
                            from PolicyIssue P
                            left join Customer C on C.CusId = P.CusId
                            left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                            left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                            left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                            left join SalesManager S on S.SalesMgId = P.SalesMgId
                            where I.InsCmpName LIKE '%'+@Company+'%' and isnull(P.PolicyNo,0) LIKE '%'+@PolicyNo+'%' and C.CusName LIKE '%'+@Client+'%' and  ISNULL(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                            order by P.RenewalDate ;
                                    
                            with A as (
                            select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE paid=1  group by PolicyId)
                            update R set cibpaid =Amount from A inner join #TEMP R on R.PolicyId = A.PolicyId;

                            with A as (
                            select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE InsPaid=1  group by PolicyId)
                            update R set InsCompPaid =Amount from A inner join #TEMP R on R.PolicyId = A.PolicyId;

                            UPDATE #TEMP SET BalanceRecivable=TotalPremium-cibpaid;

                            SELECT * FROM #TEMP";
                }
                else
                {
                    query = @"select SalesMgId into #TEMP1 from [User]  U  WHERE U.UserId=@Id 
                            union all
                            select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )
                            union all
                            select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id ))
                            union all
                            select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )))
                                     
                            select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,convert(char(3), TranDate, 0)Month,year(TranDate)Year,day(TranDate)Day,
                            C.CusName,C.Address1 CusAddress,C.EmailId,C.OfficeNo,C.MobileNo,
                            (C.EmployeeNo + ISNULL(P.AdditionEmpNo,0) - ISNULL(P.DeletionEmpNo,0))EmployeeNo,P.CustContPersonName,P.InsuredName,P.CustContDesignation,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,
                            (Select RenewalDate from PolicyIssue A where TranType='NewPolicy' AND A.PolicyId=P.PolicyId )ExpiryDate,
                            (select A.RenewalDate from PolicyIssue A  where P.TranType='RenewPolicy' and P.PolicyId=A.PolicyId)RenewalDate,
                            P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionPerc,P.CommissionAmount, S.SalesMgName,S.SalesMgCode,S.QuatarContactNo,S.OfficeEmail,P.PolicyNo,P.PolicyFee,P.PaymentTo,P.PolicySubDate,P.TranType,P.EndorcementDate,
                            (select A.PolicyNo from PolicyIssue A  where P.TranType='EndorsePolicy' and P.PolicyId=A.PolicyId)EndorcementNo,P.AdditionEmpNo,
                            CASE WHEN (P.DeletionEmpNo IS NOT NULL) and (P.DeletionEmpNo IS NOT NULL) THEN 'ADD/DEL'
                            WHEN (P.AdditionEmpNo IS NOT NULL) THEN 'ADDITION'
                            WHEN (P.DeletionEmpNo IS NOT NULL) THEN 'DELETION'
                            WHEN (P.DeletionEmpNo IS  NULL ) or (P.AdditionEmpNo IS  NULL )
                            THEN '-' END AS EndType,
                            P.DeletionEmpNo,DATEDIFF(dd,P.RenewalDate,GETDATE ()) Aging,ICActualDate,0 cibpaid, 0 BalanceRecivable,0 InsCompPaid INTO #TEMP
                            from PolicyIssue P
                            left join Customer C on C.CusId = P.CusId
                            left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                            left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                            left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                            left join SalesManager S on S.SalesMgId = P.SalesMgId
                            where  isnull(P.SalesMgId,0) IN (SELECT SalesMgId FROM #TEMP1) and
                            I.InsCmpName LIKE '%'+@Company+'%' and isnull(P.PolicyNo,0) LIKE '%'+@PolicyNo+'%' and C.CusName LIKE '%'+@Client+'%' and  ISNULL(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                            order by P.RenewalDate ;
                                    
                            with A as (
                            select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE paid=1  group by PolicyId)
                            update R set cibpaid =Amount from A inner join #TEMP R on R.PolicyId = A.PolicyId;

                            with A as (
                            select PolicyId, sum(CommittedAmt)Amount from PolicyIssueCommittedDetails WHERE InsPaid=1  group by PolicyId)
                            update R set InsCompPaid =Amount from A inner join #TEMP R on R.PolicyId = A.PolicyId;

                            UPDATE #TEMP SET BalanceRecivable=TotalPremium-cibpaid;

                            SELECT * FROM #TEMP";

                }
                return connection.Query<PolicyIssue>(query, new { Id = Id,Company = Company, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
    }
}
