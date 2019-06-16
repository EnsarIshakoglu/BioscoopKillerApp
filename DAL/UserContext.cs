using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Web;
using Interfaces;
using Interfaces.ContextInterfaces;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Enums;

namespace DAL
{
    public class UserContext : IUserContext
    {
        private readonly string _dbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";
        public IEnumerable<string> GetUserRoles(User user)
        {
            var roles = new List<string>();

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }

            return roles;
        }

        public bool CreateAccount(User user)
        {
            var isAccountCreationSuccessful = false;
            var updated = 0;

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
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

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }

            return emailInUse;
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            try
            {
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
                            Id = (int)reader["Id"],
                            Password = reader["Password"].ToString(),
                            Name = reader["Name"].ToString(),
                            SurName = reader["Surname"].ToString(),
                            Email = email
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                var location = $"{Path.GetTempPath()}\\LogFile\\LogFile.txt";
                FileStream objFilestream = new FileStream(location, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(ex.ToString());
                objStreamWriter.Close();
                objFilestream.Close();
            }

            return user;
        }
    }
}
