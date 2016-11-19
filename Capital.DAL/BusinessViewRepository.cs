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
                string query = @"select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,C.Address1 CusAddress,C.EmailId,C.MobileNo,P.CustContPersonName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,S.QuatarContactNo,S.OfficeEmail,P.PolicyNo,P.PolicySubDate,DATEDIFF(dd,P.RenewalDate,GETDATE ()) Aging
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                   	where P.PolicyId not in (select isnull(OldPolicyId,0) from PolicyIssue) and P.RenewalDate < (select dateadd(day, 30, getdate()))
                                    AND I.InsCmpName LIKE '%'+@Company+'%' and IP.InsPrdName LIKE '%'+@Product+'%' and C.CusName LIKE '%'+@Client+'%' and  S.SalesMgName LIKE '%'+@SalesManager+'%'
                                    order by P.RenewalDate ";
                return connection.Query<PolicyIssue>(query, new { Company = Company, Product = Product, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
    }
}
