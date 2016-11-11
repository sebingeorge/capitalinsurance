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
    public class InsuranceProductRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<InsuranceProductVsParameter> GetProductionParameterList()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT InsPrdParamId,InsPrdParamName
                               FROM InsuranceProductParameter ORDER BY InsPrdParamId ";
                var objProductParameter = connection.Query<InsuranceProductVsParameter>(sql).ToList<InsuranceProductVsParameter>();
                return objProductParameter;
            }
        }

        public Result Insert(InsuranceProduct model)
        {
            Result res = new Result(false);
            try
            {
                string sql;
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                     sql = @"INSERT INTO InsuranceProduct
                                   (InsPrdName
                                   ,InsPrdShortName
                                   ,InsTypeId
                                   ,InsCmpId,InsActiveDate)
                                   VALUES
                                   (@InsPrdName
                                   ,@InsPrdShortName
                                   ,@InsTypeId
                                   ,@InsCmpId,@InsActiveDate);SELECT CAST(SCOPE_IDENTITY() as int);";
                                                             
                  model.InsPrdId = connection.Query<int>(sql, model).Single();

                    foreach (var item in model.ProductParameters)
                    {
                        item.InsPrdId = model.InsPrdId;
                        sql = @"INSERT INTO InsProductVsParameter
                                   (InsPrdId,InsPrdParamId
                                   ,InsParamValue )VALUES(@InsPrdId,@InsPrdParamId,@InsParamValue);SELECT CAST(SCOPE_IDENTITY() as int);";
                                
                    }
                    int id = connection.Execute(sql, model.ProductParameters);
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
