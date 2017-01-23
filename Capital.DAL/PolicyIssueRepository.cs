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
                                    AND cast(convert(varchar(20),P.TranDate,106) as datetime) between @FromDate and @ToDate
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND isnull(P.PolicyNo,0) LIKE '%'+@PolicyNo+'%'
                                    AND isnull(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new {FromDate = FromDate,ToDate = ToDate,PolicyNo = PolicyNo,Client = Client,SalesManager = SalesManager }).ToList();
            }
        }
        public List<PolicyIssue> GetPaymentCommittedList(int Id,DateTime? FromDate, DateTime? ToDate, string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"DECLARE @SalesMgId INT = (select SalesMgId from [User]  U  WHERE U.UserId=@Id and U.UserRole=3)

                                    select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,P.CustContPersonName,P.InsuredName,I.InsCmpName,
                                    IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    where P.OldPolicyId IS NULL AND P.TranType='NewPolicy' and P.PolicyNo IS NOT NULL  and  isnull(P.SalesMgId,0)=ISNULL(@SalesMgId,isnull(P.SalesMgId,0))
                                    AND cast(convert(varchar(20),P.TranDate,106) as datetime) between @FromDate and @ToDate
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND isnull(P.PolicyNo,0) LIKE '%'+@PolicyNo+'%'
                                    AND ISNULL(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new {Id = Id, FromDate = FromDate, ToDate = ToDate, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
        public List<PolicyIssue> GetPaymentCollectionList(DateTime? FromDate, DateTime? ToDate, string PolicyNo = "", string Client = "", string SalesManager = "")
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
                                    where P.OldPolicyId IS NULL AND P.TranType='NewPolicy'and P.PayModeId IS NOT NULL and P.PolicyNo IS NOT NULL
                                    AND cast(convert(varchar(20),P.TranDate,106) as datetime) between @FromDate and @ToDate
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND isnull(P.PolicyNo,0) LIKE '%'+@PolicyNo+'%'
                                    AND ISNULL(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new { FromDate = FromDate, ToDate = ToDate, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
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
                                    DeletionEmpNo,EndorcementTypeId,TranType,OldPolicyNo,OldCompany,OldProductType,OldPremiumAmt,CreatedBy,CreatedDate,ICActualDate,TotalCommission)
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,@PolicySubDate,@EffectiveDate,@RenewalDate,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@Totalpremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,@CustContOfficeNo,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@Remarks,@FinanceManager,@PaymentTo,@PayModeId,@OldPolicyId,@CIBEffectiveDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,@TranType,@OldPolicyNo,@OldCompany,@OldProductType,@OldPremiumAmt,@CreatedBy,@CreatedDate,@ICActualDate,@TotalCommission);
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
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,TotalCommission,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,CustContOfficeNo,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,OldPolicyNo,OldCompany,OldProductType,OldPremiumAmt,CreatedBy,CreatedDate,ICActualDate,QuickBookRefNo from PolicyIssue where PolicyId=@Id";


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
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" UPDATE PolicyIssue SET 
                                    TranDate=@TranDate
                                   ,CusId=@CusId
                                   ,InsuredName=@InsuredName
                                   ,Address1=@Address1
                                   ,Address2=@Address2
                                   ,InsCmpId=@InsCmpId
                                   ,InsPrdId=@InsPrdId
                                   ,PolicySubDate=@PolicySubDate
                                   ,EffectiveDate=@EffectiveDate
                                   ,RenewalDate=@RenewalDate
                                   ,PremiumAmount=@PremiumAmount
                                   ,PolicyFee=@PolicyFee
                                   ,ExtraPremium=@ExtraPremium
                                   ,Totalpremium=@Totalpremium
                                   ,CommissionPerc=@CommissionPerc
                                   ,CommissionAmount=@CommissionAmount
                                   ,TotalCommission=@TotalCommission
                                   ,CustContPersonName=@CustContPersonName
                                   ,CustContDesignation=@CustContDesignation
                                   ,CustContEmail=@CustContEmail
                                   ,SalesMgId=@SalesMgId
                                   ,CustContMobile=@CustContMobile
                                    WHERE PolicyId=@PolicyId";
                    int id = connection.Execute(sql, model);
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
        public List<PolicyIssue> GetNewPolicyForCommitments(int Id, string trnno = "", string client = "", string insuredname = "", string insuredComp = "", string coverage = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"DECLARE @SalesMgId INT = (select SalesMgId from [User]  U  WHERE U.UserId=@Id and U.UserRole=3)


                                    select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,P.CustContPersonName,P.InsuredName,
                                    I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    where P.OldPolicyId IS NULL and P.PolicyId not in (select PolicyId from PolicyIssueCommittedDetails)  and  isnull(P.SalesMgId,0)=ISNULL(@SalesMgId,isnull(P.SalesMgId,0))
                                    and p.PolicyId like'%'+@trnno+'%'
                                    and c.CusName like '%'+@Client+'%'
									and p.InsuredName like '%'+@insuredname+'%'
									and I.InsCmpName like '%'+@insuredComp+'%'
									and IP.InsPrdName like '%'+@coverage+'%'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new {Id = Id,trnno = trnno, client = client, insuredname = insuredname, insuredComp = insuredComp, coverage = coverage }).ToList();
       
            }
        }
        public List<PolicyIssue> GetNewPolicyForPaymentCollection(string trnno = "", string client = "", string insuredname = "", string insuredComp = "", string coverage = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = @"  select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,C.CusName,P.CustContPersonName,P.InsuredName,I.InsCmpName,
                                    IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo,p.TotalPremium-isnull(sum(cr.ChequeAmt),0) BalanceAmount
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    left join PolicyIssueChequeReceived CR on Cr.PolicyId=p.PolicyId
                                    where P.OldPolicyId IS NULL AND P.TranType in ('NewPolicy','RenewPolicy')
                                    and p.PolicyId like'%'+@trnno+'%'
                                    and c.CusName like '%'+@Client+'%'
									and p.InsuredName like '%'+@insuredname+'%'
									and I.InsCmpName like '%'+@insuredComp+'%'
									and IP.InsPrdName like '%'+@coverage+'%'
                                    group by  P.PolicyId,C.CusName,P.CustContPersonName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo,p.TranPrefix,p.TranNumber
									having p.TotalPremium-isnull(sum(cr.ChequeAmt),0)>0
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query, new { trnno = trnno, client = client, insuredname = insuredname, insuredComp = insuredComp, coverage = coverage }).ToList();
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
                        sql = @"UPDATE PolicyIssue SET PaymentTo=@PaymentTo,PayModeId=@PayModeId WHERE PolicyId = @PolicyId
                                DELETE FROM PolicyIssueChequeReceived WHERE PolicyId = @PolicyId";

                        id = connection.Execute(sql, model, txn);
                        foreach (var item in model.Cheque)
                        {
                            item.PolicyId = model.PolicyId;
                            sql = @"INSERT INTO PolicyIssueChequeReceived
                                   (PolicyId,ChequeNo,ChequeDate,BankName
                                   ,BankBranch,ChequeAmt,QuickBookRefNo )VALUES(@PolicyId,@ChequeNo,@ChequeDate,@BankName,@BankBranch,@ChequeAmt,@QuickBookRefNo);SELECT CAST(SCOPE_IDENTITY() as int);";
                            id = connection.Execute(sql, item, txn);
                        }

                        foreach (var item in model.Committed)
                        {
                            if (item.paid == true)
                            {
                                item.PolicyId = model.PolicyId;
                                sql = sql = @"UPDATE PolicyIssueCommittedDetails set paid=1 WHERE CommRowId = @CommRowId";

                                id = connection.Execute(sql, item, txn);
                            }
                            if (item.InsPaid == true)
                            {
                                item.PolicyId = model.PolicyId;
                                sql = sql = @"UPDATE PolicyIssueCommittedDetails set InsPaid =1 WHERE CommRowId = @CommRowId";

                                id = connection.Execute(sql, item, txn);
                            }

                        }
                    }
                    else
                    {
                        sql = @"UPDATE PolicyIssue SET PolicyNo=@PolicyNo,Remarks=@Remarks WHERE PolicyId = @PolicyId
                                DELETE FROM PolicyIssueCommittedDetails WHERE PolicyId = @PolicyId";
                        connection.Execute(sql, model, txn);
                        foreach (var item in model.Committed)
                        {
                            item.PolicyId = model.PolicyId;
                            if (item.CommittedDate != null)
                            {
                                sql = @"INSERT INTO PolicyIssueCommittedDetails
                                   (PolicyId,CommittedDate
                                   ,CommittedAmt,paid )VALUES(@PolicyId,@CommittedDate,@CommittedAmt,@paid);SELECT CAST(SCOPE_IDENTITY() as int);";
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
        public PolicyIssue GetReceiptHdForPrint(int Id)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,P.TranDate,P.TotalPremium,P.PaymentTo,C.CusName,I.InsPrdName,IC.InsCmpName from PolicyIssue P inner join Customer C on P.CusId=C.CusId LEFT JOIN InsuranceProduct I ON I.InsPrdId=P.InsPrdId
                               LEFT JOIN InsuranceCompany IC ON IC.InsCmpId=P.InsCmpId where P.PolicyId=@Id";

                var ObjReceipt = connection.Query<PolicyIssue>(sql, new { Id = Id }).Single<PolicyIssue>();

                return ObjReceipt;
            }
        }
        public IEnumerable<PolicyIssueChequeReceived> GetReceiptChequeDetailsforPrint(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = string.Empty;
                sql = @"select  PM.PayModeName,P.TotalPremium,isnull(R.ChequeNo,'')ChequeNo,isnull(R.ChequeDate,'')ChequeDate,isnull(R.BankName,'')BankName,C.CusName  from  PolicyIssue P
                        left join PolicyIssueChequeReceived R ON R.PolicyId= P.PolicyId  left JOIN PaymentMode PM ON PM.PayModeId=P.PayModeId
                        inner join Customer C on P.CusId=C.CusId where P.PolicyId= @Id ";
                return connection.Query<PolicyIssueChequeReceived>(sql, new { Id = Id });
            }
        }
    }
}

