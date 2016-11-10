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
                                   ,ContactPerson
                                   ,ContactNo
                                   ,Designation)
                                   VALUES
                                   (@InsCmpName
                                   ,@Address1
                                   ,@Address2
                                   ,@Address3
                                   ,@Email
                                   ,@ContactPerson
                                   ,@ContactNo,@Designation);SELECT CAST(SCOPE_IDENTITY() as int);";
                                
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
