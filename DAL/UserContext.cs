using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Interfaces;
using Models;
using Models.Enums;

namespace DAL
{
    public class UserContext : IUserContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public bool CheckLogin(User user)
        {
            var loginSuccessful = false;

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("CheckLogin", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@Password", user.Password));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    loginSuccessful = true;
                }

                connection.Close();
            }

            return loginSuccessful;
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            var roles = new List<string>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetUserRolesFromUser", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader["RoleName"].ToString());
                }

                connection.Close();
            }

            return roles;
        }

        public bool CreateAccount(User user)
        {
            var isAccountCreationSuccessful = false;
            var updated = 0;

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("CreateAccount", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.SurName);

                updated = cmd.ExecuteNonQuery();

                connection.Close();
            }

            if (updated != 0)
            {
                isAccountCreationSuccessful = true;
                AddRoleToUser(user, Roles.AccountHolder.ToString());
            }

            return isAccountCreationSuccessful;
        }

        public bool IsEmailInUse(User user)
        {
            var emailInUse = false;

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("IsEmailInUse", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    emailInUse = true;
                }

                connection.Close();
            }

            return emailInUse;
        }

        public int GetUserId(User user)
        {
            var userId = -1;

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetUserId", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userId = (int)reader["Id"];
                }

                connection.Close();
            }

            return userId;
        }

        private void AddRoleToUser(User user, string roleName)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("AddRoleToUser", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@RoleName", roleName);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
