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
    public class PolicyIssueRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<PolicyIssue> GetNewPolicy(DateTime? FromDate, DateTime? ToDate, string PolicyNo = "", string Client = "", string SalesManager = "")
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
                                    where P.OldPolicyId IS NULL AND P.TranType='NewPolicy'
                                    AND CAST(P.TranDate AS date)  >=CAST(@FromDate AS date)  and CAST(P.TranDate AS date) <=CAST(@ToDate AS date)
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND P.PolicyNo LIKE '%'+@PolicyNo+'%'
                                    AND S.SalesMgName LIKE '%'+@SalesManager+'%'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new {FromDate = FromDate,ToDate = ToDate,PolicyNo = PolicyNo,Client = Client,SalesManager = SalesManager }).ToList();
            }
        }
        public Result Insert(PolicyIssue model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    model.TranNumber = PolicyIssueRepository.GetNextDocNo(model.TranType);
                    string sql = @"INSERT INTO PolicyIssue
                                   (TranPrefix,TranNumber,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,CustContOfficeNo,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,OldPolicyNo,OldCompany,OldProductType,OldPremiumAmt,CreatedBy,CreatedDate,ICActualDate )
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,@PolicySubDate,@EffectiveDate,@RenewalDate,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@Totalpremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,@CustContOfficeNo,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@Remarks,@FinanceManager,@PaymentTo,@PayModeId,@OldPolicyId,@CIBEffectiveDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,@TranType,@OldPolicyNo,@OldCompany,@OldProductType,@OldPremiumAmt,@CreatedBy,@CreatedDate,@ICActualDate );
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    model.PolicyId = connection.Query<int>(sql, model).Single();

                    foreach (var item in model.Cheque)
                    {
                        item.PolicyId = model.PolicyId;
                        sql = @"INSERT INTO PolicyIssueChequeReceived
                                   (PolicyId,ChequeNo,ChequeDate,BankName
                                   ,BankBranch,ChequeAmt )VALUES(@PolicyId,@ChequeNo,@ChequeDate,@BankName,@BankBranch,@ChequeAmt);SELECT CAST(SCOPE_IDENTITY() as int);";

                    }
                    int id = connection.Execute(sql, model.Cheque);
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
        public PolicyIssue GetNewPolicy(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select PolicyId,(TranPrefix+'/'+TranNumber)TranNumber,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,CustContOfficeNo,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,OldPolicyNo,OldCompany,OldProductType,OldPremiumAmt,CreatedBy,CreatedDate,ICActualDate from PolicyIssue where PolicyId=@Id";


                var objPolicy = connection.Query<PolicyIssue>(sql, new
                {
                    Id = Id
                }).First<PolicyIssue>();

                return objPolicy;
            }


        }
        public List<PolicyIssueChequeReceived> GetChequeDetails(int id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT * from PolicyIssueChequeReceived P
                               where P.PolicyId=@id ORDER BY InsChqRowId";
                var objCheque = connection.Query<PolicyIssueChequeReceived>(sql, new { id = id }).ToList<PolicyIssueChequeReceived>();
                return objCheque;
            }
        }
        public List<PaymentCommitments> GetCommittedDetails(int id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT * from PolicyIssueCommittedDetails P
                               where P.PolicyId=@id ORDER BY CommRowId";
                var objCheque = connection.Query<PaymentCommitments>(sql, new { id = id }).ToList<PaymentCommitments>();
                return objCheque;
            }
        }
        
        public Result Update(PolicyIssue model)
        {
            Result res = new Result(false);
            try
            {

            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
        public Result Delete(PolicyIssue model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    IDbTransaction txn = connection.BeginTransaction();

                    string query = @"DELETE FROM PolicyIssueChequeReceived WHERE PolicyId = @PolicyId;
                                     DELETE FROM PolicyIssue  OUTPUT deleted.PolicyId WHERE PolicyId = @PolicyId;";

                    int id = connection.Query<int>(query, model, txn).First();
                    txn.Commit();
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
        public Customer GetCustomerContactDetails(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = "select ContactName,Designation,EmailId,MobileNo,OfficeNo from Customer where CusId= @Id";
                return connection.Query<Customer>(query, new { Id = Id }).First<Customer>();
            }
        }
        public static string GetNextDocNo(string TYPE)
        {
            
                using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
                {
                    string query = @"select ISNULL(max(isnull(TranNumber,0)+1),1) from PolicyIssue WHERE TranType=@TYPE";
                    return connection.Query<string>(query, new { TYPE = TYPE }).Single();
                }
            }
        public List<PolicyIssue> GetNewPolicyForCommitments()
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
                                    where P.OldPolicyId IS NULL AND P.TranType='NewPolicy' and P.PolicyNo IS NULL
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query).ToList();
            }
        }
        public List<PolicyIssue> GetNewPolicyForPaymentCollection()
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
                                    where P.OldPolicyId IS NULL AND P.TranType='NewPolicy' and P.PayModeId IS NULL and P.PolicyNo IS NOT NULL
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query).ToList();
            }
        }
        public Result UpdatePaymentCommitments(PolicyIssue model)
        {
            Result res = new Result(false);
            try
            {
             
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    IDbTransaction txn = connection.BeginTransaction();
                    string sql;
                    int id = 0;
                    if (model.Type== 3)
                    {
                        sql = @"UPDATE PolicyIssue SET QuickBookRefNo=@QuickBookRefNo,PaymentTo=@PaymentTo,PayModeId=@PayModeId WHERE PolicyId = @PolicyId";

                        connection.Execute(sql, model, txn);
                        foreach (var item in model.Cheque)
                        {
                            item.PolicyId = model.PolicyId;
                            sql = @"INSERT INTO PolicyIssueChequeReceived
                                   (PolicyId,ChequeNo,ChequeDate,BankName
                                   ,BankBranch,ChequeAmt )VALUES(@PolicyId,@ChequeNo,@ChequeDate,@BankName,@BankBranch,@ChequeAmt);SELECT CAST(SCOPE_IDENTITY() as int);";
                            id = connection.Execute(sql, item, txn);
                        }
                    }
                    else
                    {
                        sql = @"UPDATE PolicyIssue SET PolicyNo=@PolicyNo,Remarks=@Remarks WHERE PolicyId = @PolicyId";

                        connection.Execute(sql, model, txn);
                        foreach (var item in model.Committed)
                        {
                            item.PolicyId = model.PolicyId;
                            if (item.CommittedDate != null)
                            {
                                sql = @"INSERT INTO PolicyIssueCommittedDetails
                                   (PolicyId,CommittedDate
                                   ,CommittedAmt )VALUES(@PolicyId,@CommittedDate,@CommittedAmt);SELECT CAST(SCOPE_IDENTITY() as int);";
                                id = connection.Execute(sql, item, txn);
                            }

                        }
                    }
                   
                    txn.Commit();
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

      
        
    }
}

