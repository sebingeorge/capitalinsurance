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
                string sql = @"SELECT IP.InsPrdParamId,IP.InsPrdParamName
                               FROM InsuranceProductParameter IP  ORDER BY IP.InsPrdParamId ";
                var objProductParameter = connection.Query<InsuranceProductVsParameter>(sql).ToList<InsuranceProductVsParameter>();
                return objProductParameter;
            }
        }
        public List<InsuranceProductVsParameter> GetProductionParameter(int id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT IP.InsPrdParamId,IP.InsPrdParamName,I.InsParamValue
                               FROM InsuranceProductParameter IP left join InsProductVsParameter I ON I.InsPrdParamId=IP.InsPrdParamId where I.InsPrdId=@id ORDER BY InsPrdParamId ";
                var objProductParameter = connection.Query<InsuranceProductVsParameter>(sql, new { id = id }).ToList<InsuranceProductVsParameter>();
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
        public List<InsuranceProduct> GetInsuranceProduct()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select I.InsPrdId,I.InsPrdName,I.InsActiveDate,IC.InsCmpName,IT.insTypeName
                from InsuranceProduct I
                left join InsuranceCompany IC on IC.InsCmpId = I.InsCmpId
                left join InsuranceType IT on IT.InsTypeId = I.InsTypeId
                order by I.InsPrdName";
                return connection.Query<InsuranceProduct>(query).ToList();
            }
        }
        public InsuranceProduct GetInsuranceProduct(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from InsuranceProduct where InsPrdId=@Id";


                var objInsProduct = connection.Query<InsuranceProduct>(sql, new
                {
                    Id = Id
                }).First<InsuranceProduct>();

                return objInsProduct;
            }


        }
        public Result Update(InsuranceProduct model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" UPDATE InsuranceProduct SET 
                                    InsPrdName=@InsPrdName
                                   ,InsPrdShortName=@InsPrdShortName
                                   ,InsTypeId=@InsTypeId
                                   ,InsCmpId=@InsCmpId
                                   ,InsActiveDate=@InsActiveDate
                                    WHERE InsPrdId=@InsPrdId";
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
        public Result Delete(InsuranceProduct model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    IDbTransaction txn = connection.BeginTransaction();
                  
                    string query = @"DELETE FROM InsProductVsParameter WHERE InsPrdId = @InsPrdId;
                                     DELETE FROM InsuranceProduct  OUTPUT deleted.InsPrdId WHERE InsPrdId = @InsPrdId;";
                     //int id = connection.Execute(query, model);
                    int id = connection.Query<int>(query, model, txn).First();
                     txn.Commit();
                 
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
