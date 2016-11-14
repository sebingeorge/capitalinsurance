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
        public Result Insert(PolicyIssue model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @"INSERT INTO PolicyIssue
                                   (TranPrefix,TranNumber,TranDate,CusId,InsuredName,Address1,Address2,InsCmpId,InsPrdId,InsCoverId,PolicySubDate,EffectiveDate,RenewalDate,
                                    PremiumAmount,PolicyFee,ExtraPremium,CommissionPerc,CommissionAmount,CustContPersonName,CustContDesignation,CustContEmail,CustContMobile,
                                    PaymentOption,SalesMgId,OperationManager,PolicyNo,FinanceManager,PaymentTo,PayModeId,OldPolicyId,CIBEffectiveDate,EndorcementDate,AdditionEmpNo,
                                    DeletionEmpNo,EndorcementTypeId,TranType,CreatedBy,CreatedDate)
                                    VALUES
                                    (@TranPrefix,@TranNumber,@TranDate,@CusId,@InsuredName,@Address1,@Address2,@InsCmpId,@InsPrdId,@InsCoverId,@PolicySubDate,@EffectiveDate,@RenewalDate,
                                    @PremiumAmount,@PolicyFee,@ExtraPremium,@CommissionPerc,@CommissionAmount,@CustContPersonName,@CustContDesignation,@CustContEmail,@CustContMobile,
                                    @PaymentOption,@SalesMgId,@OperationManager,@PolicyNo,@FinanceManager,@PaymentTo,@PayModeId,@OldPolicyId,@CIBEffectiveDate,@EndorcementDate,@AdditionEmpNo,
                                    @DeletionEmpNo,@EndorcementTypeId,'New Policy',@CreatedBy,@CreatedDate);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    int id = connection.Query<int>(sql, model).Single();
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
