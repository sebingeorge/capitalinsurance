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
    public class SalesManagerRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<SalesManager> GetSalesManagers()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<SalesManager>("select SalesMgId, SalesMgName from SalesManager").ToList();
            }
        }
        public Result Insert(SalesManager model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @"INSERT INTO SalesManager
                                   (SalesMgCode
                                   ,SalesMgName
                                   ,Gender
                                   ,MaritalStatus
                                   ,DsgId
                                   ,CountryId
                                   ,Deptment
                                   ,Location
                                   ,CurrentAddress1
                                   ,CurrentAddress2
                                   ,CurrentAddress3
                                   ,StateId
                                   ,PermanantAddress1
                                   ,PermanantAddress2
                                   ,PermanantAddress3
                                   ,PermanantState
                                   ,PermanantCountry
                                   ,QuatarContactNo
                                   ,HomeCountryContactNo
                                   ,PassportNo
                                   ,PassportIssueDate
                                   ,PassportEndDate
                                   ,VisaOrResId
                                   ,VisaIssueDate
                                   ,VisaEndDate
                                   ,DateOfJoining
                                   ,DateOfBirth
                                   ,OfficeEmail
                                   ,PersonalEmail
                                   )
                                   VALUES
                                   (@SalesMgCode
                                   ,@SalesMgName
                                   ,@Gender
                                   ,@DsgId
                                   ,@CountryId
                                   ,@Deptment
                                   ,@Location
                                   ,@CurrentAddress1
                                   ,@CurrentAddress2
                                   ,@CurrentAddress3
                                   ,@StateId
                                   ,@PermanantAddress1
                                   ,@PermanantAddress2
                                   ,@PermanantAddress3
                                   ,@PermanantState
                                   ,@PermanantCountry
                                   ,@QuatarContactNo
                                   ,@HomeCountryContactNo
                                   ,@PassportNo
                                   ,@PassportIssueDate
                                   ,@PassportEndDate
                                   ,@VisaOrResId
                                   ,@VisaIssueDate
                                   ,@VisaEndDate
                                   ,@DateOfJoining
                                   ,@DateOfBirth
                                   ,@OfficeEmail
                                   ,@PersonalEmail );SELECT CAST(SCOPE_IDENTITY() as int);";
                                   
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

        public IEnumerable<Department> FillDepartmentList()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                return connection.Query<Department>("SELECT DeptId ,DeptName  FROM Department").ToList();
            }
        }
    }
}
