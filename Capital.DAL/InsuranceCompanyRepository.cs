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
    public class InsuranceCompanyRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public Result Insert(InsuranceCompany model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @"INSERT INTO InsuranceCompany
                                   (InsCmpName
                                   ,Address1
                                   ,Address2
                                   ,Address3
                                   ,Email
                                   ,Email2
                                   ,Email3
                                   ,ContactPerson
                                   ,ContactPerson2
                                   ,ContactPerson3
                                   ,ContactNo
                                   ,ContactNo2
                                   ,ContactNo3
                                   ,Designation
                                   ,Designation2
                                   ,Designation3
                                   ,PoBox
                                   ,CrNo)
                                   VALUES
                                   (@InsCmpName
                                   ,@Address1
                                   ,@Address2
                                   ,@Address3
                                   ,@Email
                                   ,@Email2
                                   ,@Email3
                                   ,@ContactPerson
                                   ,@ContactPerson2
                                   ,@ContactPerson3
                                   ,@ContactNo,@ContactNo2,@ContactNo3,@Designation,@Designation2,@Designation3,@PoBox,@CrNo);SELECT CAST(SCOPE_IDENTITY() as int);";
                                
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

        public List<InsuranceCompany> GetCompany()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select * from InsuranceCompany
                order by InsCmpName";
                return connection.Query<InsuranceCompany>(query).ToList();
            }
        }
        public InsuranceCompany GetCompany(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from InsuranceCompany where InsCmpId=@Id";


                var objCompany = connection.Query<InsuranceCompany>(sql, new
                {
                    Id = Id
                }).First<InsuranceCompany>();

                return objCompany;
            }


        }
        public Result Update(InsuranceCompany model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" UPDATE InsuranceCompany SET 
                                    InsCmpName=@InsCmpName
                                   ,Address1=@Address1
                                   ,Address2=@Address2
                                   ,Address3=@Address3
                                   ,Email=@Email
                                   ,Email2=@Email2
                                   ,Email3=@Email3
                                   ,ContactPerson=@ContactPerson
                                   ,ContactPerson2=@ContactPerson2
                                   ,ContactPerson3=@ContactPerson3
                                   ,ContactNo=@ContactNo
                                   ,ContactNo2=@ContactNo2
                                   ,ContactNo3=@ContactNo3
                                   ,Designation=@Designation
                                   ,Designation2=@Designation2
                                   ,Designation3=@Designation3
                                   ,PoBox=@PoBox
                                   ,CrNo=@CrNo
                                   WHERE InsCmpId=@InsCmpId";
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
        public Result Delete(InsuranceCompany model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" Delete from InsuranceCompany WHERE InsCmpId=@InsCmpId";
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
