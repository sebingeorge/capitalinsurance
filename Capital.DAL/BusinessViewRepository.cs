﻿using System;
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
        public List<PolicyIssue> GetBusinessViewDetails(int Id,string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"DECLARE @SalesMgId INT = (select SalesMgId from [User]  U  WHERE U.UserId=@Id and U.UserRole=3)
                                     select P.PolicyId,Concat(P.TranPrefix,'/',P.TranNumber)StrTranNumber,convert(char(3), TranDate, 0)Month,year(TranDate)Year,day(TranDate)Day,
                                     C.CusName,C.Address1 CusAddress,C.EmailId,C.OfficeNo,C.MobileNo,
                                    (C.EmployeeNo + ISNULL(P.AdditionEmpNo,0) - ISNULL(P.DeletionEmpNo,0))EmployeeNo,P.CustContPersonName,P.InsuredName,P.CustContDesignation,I.InsCmpName,IP.InsPrdName,IC.InsCoverName,P.EffectiveDate,
                                    (Select RenewalDate from PolicyIssue A where TranType='NewPolicy' AND A.PolicyId=P.PolicyId )ExpiryDate,
                                    (select A.RenewalDate from PolicyIssue A  where P.TranType='RenewPolicy' and P.PolicyId=A.PolicyId)RenewalDate,
                                    P.PremiumAmount,P.ExtraPremium,P.Totalpremium,P.CommissionPerc,P.CommissionAmount, S.SalesMgName,S.SalesMgCode,S.QuatarContactNo,S.OfficeEmail,P.PolicyNo,P.PolicyFee,P.PaymentTo,P.PolicySubDate,P.TranType,P.EndorcementDate,
                                    (select A.PolicyNo from PolicyIssue A  where P.TranType='EndorsePolicy' and P.PolicyId=A.PolicyId)EndorcementNo,P.AdditionEmpNo,
                                    CASE WHEN (P.DeletionEmpNo IS NOT NULL) and (P.DeletionEmpNo IS NOT NULL) THEN 'ADD/DEL'
                                    WHEN (P.AdditionEmpNo IS NOT NULL) THEN 'ADDITION'
                                    WHEN (P.DeletionEmpNo IS NOT NULL) THEN 'DELETION'
                                    WHEN (P.DeletionEmpNo IS  NULL ) or (P.AdditionEmpNo IS  NULL )
                                    THEN '-' END AS EndType,
                                    P.DeletionEmpNo,DATEDIFF(dd,P.RenewalDate,GETDATE ()) Aging,ICActualDate
                                    from PolicyIssue P
                                    left join Customer C on C.CusId = P.CusId
                                    left join InsuranceCompany I on I.InsCmpId = P.InsCmpId
                                    left join InsuranceProduct IP on IP.InsPrdId = P.InsPrdId
                                    left join InsuranceCoverage IC on IC.InsCoverId = P.InsCoverId
                                    left join SalesManager S on S.SalesMgId = P.SalesMgId
                                   	where  isnull(P.SalesMgId,0)=ISNULL(@SalesMgId,isnull(P.SalesMgId,0)) and
                                    I.InsCmpName LIKE '%'+@Company+'%' and P.PolicyNo LIKE '%'+@PolicyNo+'%' and C.CusName LIKE '%'+@Client+'%' and  ISNULL(S.SalesMgName,0) LIKE '%'+@SalesManager+'%'
                                    order by P.RenewalDate ";
                return connection.Query<PolicyIssue>(query, new { Id = Id,Company = Company, PolicyNo = PolicyNo, Client = Client, SalesManager = SalesManager }).ToList();
            }
        }
    }
}
