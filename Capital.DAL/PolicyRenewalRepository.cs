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
  public  class PolicyRenewalRepository : BaseRepository
  
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<PolicyIssue> GetNewPolicyForRenewal(DateTime? FromDate, DateTime? ToDate,string PolicyNo="" ,string Client = "", string SalesManager = "")
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
                                   	where P.PolicyId not in (select isnull(OldPolicyId,0) from PolicyIssue) 
                                    AND CAST(P.RenewalDate AS date)  >=CAST(@FromDate AS date)  and CAST(P.RenewalDate AS date) <=CAST(@ToDate AS date)
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND P.PolicyNo LIKE '%'+@PolicyNo+'%'
                                    AND S.SalesMgName LIKE '%'+@SalesManager+'%'
                                    order by P.RenewalDate ";
                return connection.Query<PolicyIssue>(query, new { FromDate = FromDate, ToDate = ToDate, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
        public PolicyIssue GetNewPolicyForRenewal(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select PolicyId,TranPrefix,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorcementDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate from PolicyIssue where PolicyId=@Id";


                var objPolicy = connection.Query<PolicyIssue>(sql, new
                {
                    Id = Id
                }).First<PolicyIssue>();

                return objPolicy;
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
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorcementDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate)
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,@PolicySubDate,@EffectiveDate,@RenewalDate,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@Totalpremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@Remarks,@FinanceManager,@PaymentTo,@PayModeId,@PolicyId,@CIBEffectiveDate,@EndorcementDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,'RenewPolicy',@CreatedBy,@CreatedDate);
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
        public List<PolicyIssue> GetRenewalPolicy()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select P.PolicyId,P.TranNumber,C.CusName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    where P.OldPolicyId IS NOT NULL and P.TranType = 'Renew Policy'
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query).ToList();
            }
        }
        public DateTime GetFromDate()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = @"select dateadd(day, -30, getdate())";
                return connection.Query<DateTime>(qry).First();
            }
        }
        public DateTime GetToDate()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = @"select dateadd(day, 30, getdate())";
                return connection.Query<DateTime>(qry).First();
            }
        }
      
      
    }
}
