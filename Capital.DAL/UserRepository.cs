using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Capital.Domain;
using System.Data;
using Dapper;

namespace Capital.DAL
{
    public class UserRepository: BaseRepository
    {
        static string dataConnection = GetConnectionString("CibConnection");
        public List<UserRole> GetUserRole()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "select RoleId, RoleName from UserRole";
                return connection.Query<UserRole>(sql).ToList();
            }
        }
        public int InsertUser(User user)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                try
                {
                    string sql = "INSERT INTO [dbo].[User](UserName, UserEmail, UserPassword, UserSalt, UserRole,SalesMgId,Reporting)";
                    sql += " VALUES(@UserName, @UserEmail, @UserPassword, @UserSalt, @UserRole, @SalesMgId,@Reporting);";
                    sql += " SELECT CAST(SCOPE_IDENTITY() as int);";

                    var id = connection.Query<int>(sql, user).Single();

                    foreach (var item in user.Module)
                    {
                        if(item.isPermission == 1)
                        {
                            sql = "Insert into ModuleVsUser(UserId, ModuleId) values (@UserId, @ModuleId);";
                            connection.Query<int>(sql, new { UserId = id, ModuleId = item.ModuleId });
                        }
                    }
                    InsertFormPermission(user.Forms, id);
                    //InsertLoginHistory(dataConnection, user.CreatedBy, "Create", "Unit", id.ToString(), "0");
                    return id;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        private int InsertFormPermission(List<FormsVsUser> Forms, int UserId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"DELETE FROM UserVsForms WHERE UserId = @UserId";
                    connection.Execute(query, new { UserId = UserId }, txn);
                    query = @"INSERT INTO UserVsForms (UserId, FormId)
                              VALUES (@UserId, @FormId)";
                    foreach (var item in Forms)
                    {
                        connection.Execute(query, new { UserId = UserId, FormId = item.FormId }, txn);
                    }
                    txn.Commit();
                    return 1;
                }
                catch
                {
                    txn.Rollback();
                    return 0;
                }
            }
        }
        public int UpdateUser(User user)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                try
                {
                    string sql = "Update [User] set UserEmail = @UserEmail, UserRole = @UserRole, SalesMgId = @SalesMgId,Reporting = @Reporting";
                    if (user.UserPassword != null && user.UserPassword != "")
                    {
                        sql += ", UserPassword = @UserPassword, @UserSalt = UserSalt";
                    }
                    sql += " where UserId = @UserId;";
                    connection.Query(sql, user);

                    connection.Query("delete from ModuleVsUser where UserId = " + user.UserId.ToString());

                    foreach (var item in user.Module)
                    {
                        if (item.isPermission == 1)
                        {
                            sql = "Insert into ModuleVsUser(UserId, ModuleId) values (@UserId, @ModuleId);";
                            connection.Query<int>(sql, new { UserId = user.UserId, ModuleId = item.ModuleId });
                        }
                    }
                    InsertFormPermission(user.Forms, user.UserId ?? 0);
                }
                catch
                {
                    return 0;
                }
            }
            //InsertLoginHistory(dataConnection, user.CreatedBy, "Update", "Unit", user.UserId.ToString(), "0");
            return user.UserId ?? 0;
        }
        public User GetUserByUserNameAndPassword(string username, string password)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "select * from [User] where UserName='" + username + "' and UserPassword='" + password + "'";
                var user = new User();
                try
                {
                    user = connection.Query<User>(sql).Single();
                }
                catch
                {

                }
                return user;
            }
        }
        public User GetUserById(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "select * from [User] where UserId=" + Id.ToString();
                var user = new User();
                try
                {
                    user = connection.Query<User>(sql).Single();
                }
                catch
                {

                }
                return user;
            }
        }
        public bool IsValidUser(int UserId, string Username, string ControllName, string ActionName, int OrganizationId, string SessionID)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                if (ActionName != "LoadQuickView" && ControllName != "Home")
                {
                    string sql = @"insert into TransactionHistory(UserId, TransTime, Mode, Form, OrganizationId, SessionID)
                               values('" + UserId.ToString() + "', GetDate(), '" + ActionName + "', '" + ControllName + "', '" + OrganizationId.ToString() + "','" + SessionID + "')";
                    connection.Query(sql);
                }
            }
            return true;
        }
        public void InsertLoginHistory(User user, string sessionId, string ipAddress, string OrganizationId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "Insert into LoginHistory(UserId, LogIn, SessionID, IpAddress, OrganizationId)";
                sql += " values('" + user.UserId.ToString() + "', GetDate(), '" + sessionId + "','" + ipAddress + "','" + OrganizationId + "')";
                connection.Query(sql);
            }
        }
        public void UpdateLoginHistory(string sessionId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "update LoginHistory set LogoutTime = Getdate() where SessionID = " + sessionId;
            }
        }
        public List<User> GetUserAndModuleInfoList()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
//                string sql = @"select U.UserId, U.UserName, U.UserEmail, UR.RoleName from [User] U
//                left join UserRole UR on U.UserRole = UR.RoleId";
                string sql = @"SELECT [UserId]
                ,[UserName]
                ,[UserEmail]
                ,[UserPassword]
                ,[UserSalt]
                , UR.RoleName
                , ModuleNames = STUFF((SELECT ',' + M.ModuleName AS[text()]
                FROM Modules M join ModuleVsUser MU

                on M.ModuleId = MU.ModuleId
                WHERE
                MU.UserId = U.UserId
                FOR XML PATH('')
                ), 1, 1,'')
                FROM[dbo].[User] U left join UserRole UR on U.UserRole = UR.RoleId";
                return connection.Query<User>(sql).ToList();
            }
        }
        public List<Modules> GetModules(int? Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "Select ModuleId, ModuleName from Modules";
                if((Id ?? 0) >0)
                {
                    sql = @"Select Modules.ModuleId, ModuleName, isPermission = case when ModuleVsUser.UserId is null then 0 else 1 end
                    from Modules left join ModuleVsUser on Modules.ModuleId = ModuleVsUser.ModuleId
                    and ModuleVsUser.UserId = " + (Id ?? 0).ToString();
                }
                return connection.Query<Modules>(sql).ToList();
            }
        }
        public IEnumerable<FormsVsUser> GetFormsVsUser(int UserId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"SELECT
	                                F.FormId,
	                                F.FormName,
	                                F.ModuleId,
	                                M.ModuleName,
	                                CASE WHEN UserId IS NULL THEN 0 ELSE 1 END AS hasPermission
                                FROM Forms F
	                                LEFT JOIN UserVsForms UF ON F.FormId = UF.FormId AND UserId = @UserId
	                                INNER JOIN Modules M ON F.ModuleId = M.ModuleId";
                return connection.Query<FormsVsUser>(query, new { UserId = UserId });
            }
        }
        public RegisterViewModel GetUserInfo(int? Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT [UserId]
                ,[UserName]
                ,[UserEmail]
                ,[UserPassword]
                ,[UserSalt]
                ,[UserRole]
                FROM[dbo].[User] U Where U.UserId=" + Id;
                return connection.Query<RegisterViewModel>(sql).FirstOrDefault();
            }
        }

        public IEnumerable<FormPermission> GetFormPermissions(int UserId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"SELECT F.FormId FROM Forms F
                                INNER JOIN UserVsForms UF ON F.FormId = UF.FormId 
                                WHERE UserId = @UserId";
                return connection.Query<FormPermission>(query, new { UserId = UserId });
            }
        }
    }
}
