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
  public  class FinancialYearRepository:BaseRepository
    {
      static string dataConnection = GetConnectionString("CibConnection");
      public DateTime GetFinStartDate()
      {
          using (IDbConnection connection = OpenConnection(dataConnection))
          {

              string qry = @"select FyStartDate from [dbo].[FinancialYear] where year(FyStartDate)=year(GETDATE())";
                 
   
              return connection.Query<DateTime>(qry).First();
          }
      }
    }
}
