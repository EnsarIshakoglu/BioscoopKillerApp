using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DAL.Contexts
{
    public class RoomContext : IRoomContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public IEnumerable<string> GetAllRoomTypes()
        {
            var roomTypes = new List<string>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"select tr.[Name] from TypeRoom tr", connection);
                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    roomTypes.Add(reader["Name"]?.ToString());
                }

                connection.Close();
            }

            return roomTypes;
        }

        public IEnumerable<int> GetRoomIdsByRoomType(string roomType)
        {
            var roomIds = new List<int>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"select r.ID from Room r " +
                                                $"inner join TypeRoom tr on r.TypeRoomID = tr.ID " +
                                                $"where tr.[Name] = '{roomType}'", connection);
                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    roomIds.Add((int)reader["ID"]);
                }

                connection.Close();
            }

            return roomIds;
        }
    }
}
