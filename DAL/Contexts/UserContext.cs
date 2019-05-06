using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;
using Models.Enums;

namespace DAL.Contexts
{
    public class UserContext : IUserContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public bool Login(User user)
        {
            bool loginSuccesfull = false;

            using (SqlConnection conn = new SqlConnection(_dbConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand($"SELECT TOP 1 Id, [E-mail],Password from dbo.[User] WHERE [E-mail] = '{user.Email}' and Password = '{user.Password}'", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loginSuccesfull = true;
                }
                reader.Close();
                conn.Close();
            }
            return loginSuccesfull;
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            var roles = new List<string>();

            using (SqlConnection conn = new SqlConnection(_dbConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand($"select U.[E-mail], R.RoleName from dbo.User_Roles UR\r\ninner join [User] U on UR.UserID = U.ID\r\ninner join Roles R on UR.RoleID = R.ID\r\nwhere U.[E-mail] = '{user.Email}'", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader["RoleName"].ToString());
                }
                reader.Close();
                conn.Close();
            }

            return roles;
        }

        public bool CreateAccount(User user)
        {
            var isAccountCreationSuccessful = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var sqlCommand =
                        new SqlCommand(
                            $"INSERT INTO dbo.[User] ([E-mail], [Password], [Name], [Surname]) VALUES (@Email, @Password, @Name, @Surname)",
                            connection);
                    sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", user.Password);
                    sqlCommand.Parameters.AddWithValue("@Name", user.Name);
                    sqlCommand.Parameters.AddWithValue("@Surname", user.SurName);
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                //loggen
                isAccountCreationSuccessful = false;
            }

            if (isAccountCreationSuccessful)
            {
                AddRole(user, Roles.AccountHolder.ToString());
            }

            return isAccountCreationSuccessful;
        }

        public bool IsEmailInUse(User user)
        {
            var emailInUse = false;

            using (SqlConnection conn = new SqlConnection(_dbConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand($"select * from [user] u \r\nwhere \r\nu.[E-mail] = \'{user.Email}\'", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    emailInUse = true;
                }
                reader.Close();
                conn.Close();
            }

            return emailInUse;
        }

        private void AddRole(User user, string roleName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var sqlCommand =
                        new SqlCommand(
                            $"INSERT INTO dbo.User_Roles (UserID, RoleID) VALUES ((select u.ID from [user] u where u.[E-mail] = @Email), (select r.ID from Roles r where r.RoleName = @RoleName))",
                            connection);

                    sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                    sqlCommand.Parameters.AddWithValue("@RoleName", roleName);
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                //loggen
            }
        }
    }
}
