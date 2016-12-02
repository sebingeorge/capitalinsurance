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
    }
}
