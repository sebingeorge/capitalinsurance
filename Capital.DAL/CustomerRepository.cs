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
    public class CustomerRepository: BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public Result Insert(Customer model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @"INSERT INTO Customer
                                   (CusName
                                   ,CusShortName
                                   ,RegionId
                                   ,SalesMgId
                                   ,CusCatId
                                   ,EmployeeNo
                                   ,PremisNo
                                   ,ContactName
                                   ,Designation
                                   ,OfficeNo
                                   ,MobileNo
                                   ,EmailId
                                   ,StateId
                                   ,CountryId
                                   ,Address1
                                   ,Address2
                                   ,Address3
                                   ,CreditPeriod
                                   ,CreditPeriod2
                                   ,CreditPeriod3
                                   ,CreditPeriod4
                                   ,CreditAmount)
                             VALUES
                                   (@CusName
                                   ,@CusShortName
                                   ,@RegionId
                                   ,@SalesMgId
                                   ,@CusCatId
                                   ,@EmployeeNo
                                   ,@PremisNo
                                   ,@ContactName
                                   ,@Designation
                                   ,@OfficeNo
                                   ,@MobileNo
                                   ,@EmailId
                                   ,@StateId
                                   ,@CountryId
                                   ,@Address1
                                   ,@Address2
                                   ,@Address3
                                   ,@CreditPeriod
                                   ,@CreditPeriod2
                                   ,@CreditPeriod3
                                   ,@CreditPeriod4
                                   ,@CreditAmount);SELECT CAST(SCOPE_IDENTITY() as int);";
                    int id = connection.Query<int>(sql, model).Single();
                    if(id>0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch(Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
        public List<Customer> GetCustomer()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select C.CusId, C.CusName, C.CusShortName,
                R.RegionName, SM.SalesMgName, CC.CusCategory, S.StateName,
                CT.CountryName, C.ContactName, C.Designation, C.OfficeNo, C.MobileNo,
                C.EmailId
                from Customer C
                left join Region R on R.RegionId = C.RegionId
                left join SalesManager SM on SM.SalesMgId = C.SalesMgId
                left join CustomerCategory CC on CC.CusCatId = C.CusCatId
                left join [State] S on S.StateId = C.StateId
                left join Country CT on CT.CountryId = C.CountryId
                order by C.CusName";
                return connection.Query<Customer>(query).ToList();
            }
        }
        public Customer GetCustomer(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from Customer where CusId=@Id";
                       

                var objCustomer = connection.Query<Customer>(sql, new
                {
                    Id = Id
                }).First<Customer>();

                return objCustomer;
            }


        }
        public Result Update(Customer model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" UPDATE Customer SET 
                                   CusName=@CusName
                                   ,CusShortName=@CusShortName
                                   ,RegionId=@RegionId
                                   ,SalesMgId=@SalesMgId
                                   ,CusCatId=@CusCatId
                                   ,EmployeeNo=@EmployeeNo
                                   ,PremisNo=@PremisNo
                                   ,ContactName=@ContactName
                                   ,Designation=@Designation
                                   ,OfficeNo=@OfficeNo
                                   ,MobileNo=@MobileNo
                                   ,EmailId=@EmailId
                                   ,StateId=@StateId
                                   ,CountryId=@CountryId
                                   ,Address1=@Address1
                                   ,Address2=@Address2
                                   ,Address3=@Address3
                                   ,CreditPeriod=@CreditPeriod
                                   ,CreditPeriod2=@CreditPeriod2
                                   ,CreditPeriod3=@CreditPeriod3
                                   ,CreditPeriod4=@CreditPeriod4
                                   ,CreditAmount=@CreditAmount WHERE CusId=@CusId";
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
        public Result Delete(Customer model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" Delete from Customer WHERE CusId=@CusId";
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

    }
}
