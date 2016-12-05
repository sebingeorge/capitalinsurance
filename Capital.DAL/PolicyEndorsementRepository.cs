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
    public class PolicyEndorsementRepository : BaseRepository
    {
     static string dataConnection = GetConnectionString("CibConnection");
     public List<PolicyIssue> GetNewPolicyForEndorse(DateTime? FromDate, DateTime? ToDate, string PolicyNo = "", string Client = "", string SalesManager = "")
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
                                   	where P.PolicyId not in (select isnull(OldPolicyId,0) from PolicyIssue) and P.PayModeId IS NOT NULL and P.PolicyNo IS NOT NULL
                                    AND CAST(P.TranDate AS date)  >=CAST(@FromDate AS date)  and CAST(P.TranDate AS date) <=CAST(@ToDate AS date)
                                    AND C.CusName LIKE '%'+@Client+'%'
                                    AND P.PolicyNo LIKE '%'+@PolicyNo+'%'
                                    AND S.SalesMgName LIKE '%'+@SalesManager+'%'
                                    order by P.TranNumber desc";
             return connection.Query<PolicyIssue>(query, new { FromDate = FromDate, ToDate = ToDate, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
         }
     }
     public PolicyIssue GetNewPolicyForEndorse(int Id)
     {
         using (IDbConnection connection = OpenConnection(dataConnection))
         {
             string sql = @"select PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)TranNumber,TranPrefix,P.TranNumber StrTranNumber,TranDate,P.CusId,C.EmployeeNo,InsuredName,P.Address1,P.Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,P.SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorcementDate,AdditionEmpNo,
                                    DeletionEmpNo,(C.EmployeeNo+AdditionEmpNo-DeletionEmpNo)TotalEmployes,EndorsementNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate,QuickBookRefNo from PolicyIssue P
                                    inner join Customer C on C.CusId=P.CusId
                                    where PolicyId=@Id";
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
                
                 string sql = @"INSERT INTO PolicyIssue
                                   (TranPrefix,TranNumber,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorsementNo,EndorcementDate,ICActualDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate)
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@Totalpremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@Remarks,@FinanceManager,@PaymentTo,@PayModeId,@PolicyId,@CIBEffectiveDate,@EndorsementNo,@EndorcementDate,@ICActualDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,'EndorsePolicy',@CreatedBy,@CreatedDate);
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
    }
}
