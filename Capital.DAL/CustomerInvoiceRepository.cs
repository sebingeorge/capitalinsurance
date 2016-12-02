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
    public class CustomerInvoiceRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<PolicyIssue> GetPendingPoilcyforInvoice(int ClientId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select  C.CusName,I.InsCmpName,P.TranType,P.PolicyNo,P.PolicySubDate,P.EffectiveDate,P.RenewalDate,P.PolicyId
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    where P.CusId = ISNULL(NULLIF(@ClientId, 0), P.CusId)";

                return connection.Query<PolicyIssue>(query, new { ClientId = ClientId }).ToList();
            }
        }

        public List<CustomerInvoiceItem> GetPolicyDetailsForInvoiceDetails(List<int> PolicyIds)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT I.InsCmpName,P.EffectiveDate,P.InsuredName,P.TranType,P.PolicyNo, 1 Qty,TotalPremium Rate,0 Discount,0 Amount,'Nos'Unit from PolicyIssue P  left join InsuranceCompany I on I.InsCmpId = P.InsCmpId  WHERE P.PolicyId IN @PolicyIds";

                var objPendingInv = connection.Query<CustomerInvoiceItem>(sql, new { PolicyIds = PolicyIds }).ToList<CustomerInvoiceItem>();

                return objPendingInv;
            }
        }
        public CustomerInvoice GetPolicyDetailsForInvoice(List<int> PolicyIds)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @" SELECT DISTINCT C.CusName,C.Address1,C.Address2,C.Address3 from PolicyIssue P INNER JOIN Customer C ON P.CusId=C.CusId WHERE P.PolicyId IN @PolicyIds";

                var objPendingInv = connection.Query<CustomerInvoice>(sql, new { PolicyIds = PolicyIds }).Single<CustomerInvoice>();

                return objPendingInv;
            }
        }
        public static string GetNextDocNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(CusInvoiceId,0)+1),1) from CustomerInvoice";
                return connection.Query<string>(query).Single();
            }
        }
    }
}
