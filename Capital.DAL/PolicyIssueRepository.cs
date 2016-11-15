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
        public List<PolicyIssue> GetNewPolicy()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select P.TranNumber,C.CusName,P.InsuredName,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,P.RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionAmount, S.SalesMgName,P.PolicyNo
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                    order by P.TranNumber";
                return connection.Query<PolicyIssue>(query).ToList();
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
                                   (TranPrefix,TranNumber,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,Totalpremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,Remarks,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorcementDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate)
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,@PolicySubDate,@EffectiveDate,@RenewalDate,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@Totalpremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@Remarks,@FinanceManager,@PaymentTo,@PayModeId,@OldPolicyId,@CIBEffectiveDate,@EndorcementDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,'New Policy',@CreatedBy,@CreatedDate);
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
