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
    public class DropdownRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");

        public List<Dropdown> GetDesignation()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT DsgId Id, DsgName Name FROM Designation").ToList();
            }
        }
        public List<Dropdown> GetInsuranceType()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT InsTypeId Id, insTypeName Name FROM InsuranceType").ToList();
            }
        }
        public List<Dropdown> GetInsuranceCompany()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT InsCmpId Id, InsCmpName Name FROM InsuranceCompany").ToList();
            }
        }
        public List<Dropdown> GetCustomer()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT CusId Id, CusName Name FROM Customer").ToList();
            }
        }
        public List<Dropdown> GetCustomerFrmPolicy()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT CusId Id, CusName Name FROM Customer where CusId in (select CusId from PolicyIssue)").ToList();
            }
        }
        public List<Dropdown> GetInsuranceProduct()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT InsPrdId Id, InsPrdName Name FROM InsuranceProduct").ToList();
            }
        }
        public List<Dropdown> GetProductType()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT InsCoverId Id, InsCoverName Name FROM InsuranceCoverage").ToList();
            }
        }
        public List<Dropdown> GetPaymentMode()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT PayModeId Id, PayModeName Name FROM PaymentMode").ToList();
            }
        }
        public List<Dropdown> FillFinYear()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT FyId Id, FyName Name FROM FinancialYear").ToList();
            }
        }
        public List<Dropdown> Type()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT FyId Id, FyName Name FROM FinancialYear").ToList();
            }
        }
        public List<Dropdown> GetDailyActivityComboforEmployee(int id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select SalesMgId into #TEMP from [User]  U  WHERE U.UserId=@Id
                union all
                select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )
                union all
                select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id ))
                union all
                select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U where Reporting in (select SalesMgId from [User]  U  WHERE U.UserId=@Id )))

                select T.SalesMgId Id,S.SalesMgName Name from #TEMP T inner join SalesManager S on S.SalesMgId=T.SalesMgId";

                return connection.Query<Dropdown>(query, new { id = id }).ToList();
            }
        }
        public List<Dropdown> GetSalesManagers()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT SalesMgId Id, SalesMgName Name FROM SalesManager where SalesMgId not in (SELECT SalesMgId FROM [User]) ").ToList();
            }
        }
        }
}
