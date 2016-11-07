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
    public class StateRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<State> GetState()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<State>("select StateId, StateName from State").ToList();
            }
        }
    }
}
