using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Enums;

namespace DAL
{
    public class UserContext : IUserContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";
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
            }

            if (updated != 0)
            {
                isAccountCreationSuccessful = true;
            }

            return isAccountCreationSuccessful;
        }

        public bool IsEmailInUse(string email)
        {
            var emailInUse = false;

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("IsEmailInUse", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", email));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    emailInUse = true;
                }
            }

            return emailInUse;
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetUserByEmail", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Email", email));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User
                    {
                        Id = (int) reader["Id"],
                        Password = reader["Password"].ToString(),
                        Name = reader["Name"].ToString(),
                        SurName = reader["Surname"].ToString(),
                        Email = email
                    };
                }
            }

            return user;
        }
    }
}
