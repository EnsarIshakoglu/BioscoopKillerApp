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
        private readonly string _dbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";

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
                        AiringTime = (reader["AiringTime"] as DateTime?).GetValueOrDefault(),
                        Id = (reader["AiringMovieId"] as int?).GetValueOrDefault(),
                        Movie = new Movie
                        {
                            Title = reader["MovieName"]?.ToString(),
                            Id = (reader["MovieId"] as int?).GetValueOrDefault()
                        },
                        Room = new Room
                        {
                            Number = (reader["RoomId"] as int?).GetValueOrDefault(),
                            Type = reader["RoomType"]?.ToString()
                        }
                    });
                }
            }

            return airingMovies;
        }
    }
}
