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
        public List<PolicyIssue> GetBusinessViewDetails(string Company = "", string Product = "", string Client = "", string SalesManager = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,convert(char(3), TranDate, 0)Month,year(TranDate)Year,day(TranDate)Day,C.CusName,C.Address1 CusAddress,C.EmailId,C.OfficeNo,C.MobileNo,P.CustContPersonName,P.InsuredName,P.CustContDesignation,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionPerc,P.CommissionAmount, S.SalesMgName,S.SalesMgCode,S.QuatarContactNo,S.OfficeEmail,P.PolicyNo,P.PaymentTo,P.PolicySubDate,P.TranType,P.EndorcementDate,
                                    (select A.PolicyNo from PolicyIssue A  where P.TranType='EndorsePolicy' and P.PolicyId=A.PolicyId)EndorcementNo,P.AdditionEmpNo,
                                    CASE WHEN (P.DeletionEmpNo IS NOT NULL) and (P.DeletionEmpNo IS NOT NULL) THEN 'ADD/DEL'
                                    WHEN (P.AdditionEmpNo IS NOT NULL) THEN 'ADDITION'
                                    WHEN (P.DeletionEmpNo IS NOT NULL) THEN 'DELETION'
                                    WHEN (P.DeletionEmpNo IS  NULL ) or (P.AdditionEmpNo IS  NULL )
                                    THEN '-' END AS EndType,
                                    P.DeletionEmpNo,DATEDIFF(dd,P.RenewalDate,GETDATE ()) Aging
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                   	where  P.RenewalDate < (select dateadd(day, 30, getdate()))
                                    AND I.InsCmpName LIKE '%'+@Company+'%' and IP.InsPrdName LIKE '%'+@Product+'%' and C.CusName LIKE '%'+@Client+'%' and  S.SalesMgName LIKE '%'+@SalesManager+'%'
                                    order by P.RenewalDate ";
                return connection.Query<PolicyIssue>(query, new { Company = Company, Product = Product, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
    }
}
