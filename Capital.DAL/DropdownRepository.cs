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


    }
}
