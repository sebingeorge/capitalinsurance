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
                                    left join CustomerInvoiceItem CI ON CI.PolicyId =P.PolicyId
                                    where CI.PolicyId IS NULL AND P.CusId = ISNULL(NULLIF(@ClientId, 0), P.CusId)";

                return connection.Query<PolicyIssue>(query, new { ClientId = ClientId }).ToList();
            }
        }

        public List<CustomerInvoiceItem> GetPolicyDetailsForInvoiceDetails(List<int> PolicyIds)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT I.InsCmpName,P.EffectiveDate,P.InsuredName,P.TranType,P.PolicyNo,P.PolicyId,TotalPremium  from PolicyIssue P  left join InsuranceCompany I on I.InsCmpId = P.InsCmpId  WHERE P.PolicyId IN @PolicyIds";

                var objPendingInv = connection.Query<CustomerInvoiceItem>(sql, new { PolicyIds = PolicyIds }).ToList<CustomerInvoiceItem>();

                return objPendingInv;
            }
        }
        public CustomerInvoice GetPolicyDetailsForInvoice(List<int> PolicyIds)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @" SELECT DISTINCT C.CusId,C.CusName,C.Address1,C.Address2,C.Address3 from PolicyIssue P INNER JOIN Customer C ON P.CusId=C.CusId WHERE P.PolicyId IN @PolicyIds";

                var objPendingInv = connection.Query<CustomerInvoice>(sql, new { PolicyIds = PolicyIds }).Single<CustomerInvoice>();

                return objPendingInv;
            }
        }
        public static string GetNextDocNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(CusInvoiceRefNo,0)+1),1) from CustomerInvoice";
                return connection.Query<string>(query).Single();
            }
        }

        public Result Insert(CustomerInvoice model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    model.CusInvoiceRefNo = CustomerInvoiceRepository.GetNextDocNo();
                    model.TotalAmount = model.Items.Sum(m => m.TotalPremium);
                    string sql = @"INSERT INTO CustomerInvoice
                                   (CusInvoiceRefNo,CusInvoiceDate,CusId,SpecialRemarks,TotalAmount,CreatedBy,CreatedDate )
                                   VALUES
                                   (@CusInvoiceRefNo,@CusInvoiceDate,@CusId,@SpecialRemarks,@TotalAmount,@CreatedBy,@CreatedDate );
                                   SELECT CAST(SCOPE_IDENTITY() as int);";
                    model.CusInvoiceId = connection.Query<int>(sql, model).Single();
                    foreach (var item in model.Items)
                    {
                        item.CusInvoiceId = model.CusInvoiceId;
                        sql = @"INSERT INTO CustomerInvoiceItem
                                (CusInvoiceId,PolicyId)VALUES(@CusInvoiceId,@PolicyId);SELECT CAST(SCOPE_IDENTITY() as int);";
                    }
                    int id = connection.Execute(sql, model.Items);
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
        public List<CustomerInvoice> GetCustomerInvoiceList()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select CI.CusInvoiceRefNo,CI.CusInvoiceDate,C.CusName,CI.TotalAmount from CustomerInvoice CI inner join Customer C ON CI.CusId=C.CusId ";
                return connection.Query<CustomerInvoice>(query).ToList();
            }
        }
    }
}
