using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Interfaces;
using Models;

namespace DAL
{
    public class RoomContext : IRoomContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public IEnumerable<string> GetAllRoomTypes()
        {
            var roomTypes = new List<string>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAllRoomTypes", connection) { CommandType = CommandType.StoredProcedure };

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roomTypes.Add(reader["Name"]?.ToString());
                }

                connection.Close();
            }

            return roomTypes;
        }

        public IEnumerable<Room> GetRoomsByRoomType(string roomType)
        {
            var rooms = new List<Room>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetRoomsByRoomType", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomType", roomType));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rooms.Add(new Room
                    {
                        Number = (int)reader["ID"]
                    });
                }

                connection.Close();
            }

            return rooms;
        }
        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            var airingMovies = new List<AiringMovie>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAiringsByRoomType2", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomType", roomType));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        AiringTime = reader["AiringTime"] as DateTime?,
                        Id = reader["AiringMovieId"] as int?,
                        Movie = new Movie
                        {
                            Title = reader["MovieName"]?.ToString(),
                            Id = reader["MovieId"] as int?
                        },
                        Room = new Room
                        {
                            Number = reader["RoomId"] as int?,
                            Type = reader["RoomType"]?.ToString()
                        }
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }
    }
}
