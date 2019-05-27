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
    public class AiringMovieContext : IAiringMovieContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            var airingMovies = new List<AiringMovie>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand($"GetAiringsFromMovie", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@MovieId", movie.Id));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        Id = (int)reader["AiringID"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = movie,
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"],(int)reader["SeatsPerRow"]),
                        Price = (decimal)reader["Price"]
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }

        public AiringMovie GetAiringMovieById(int id)
        {
            var airingMovie = new AiringMovie();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAiringById", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@PlanningId", id));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovie = new AiringMovie
                    {
                        Id = (int)reader["AiringID"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"], (int)reader["SeatsPerRow"]),
                        Price = (decimal)reader["Price"]
                    };
                }

                connection.Close();
            }

            return airingMovie;
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            var airingMovies = new List<AiringMovie>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAiringsByRoomType", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomType", roomType));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        Id = (int)reader["AiringID"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = new Movie
                        {
                            Title = reader["Name"]?.ToString(),
                            Id = (int)reader["MovieId"]
                        },
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"], (int)reader["SeatsPerRow"]),
                        Price = (decimal)reader["Price"]
                    });
                    
                }

                connection.Close();
            }

            return airingMovies;
        }

        public void AddAiringMovie(AiringMovie airingMovie)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("AddAiring", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomId", airingMovie.Room.Number));
                cmd.Parameters.Add(new SqlParameter("@AiringTime", airingMovie.AiringTime));
                cmd.Parameters.Add(new SqlParameter("@MovieId", airingMovie.Movie.Id));

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public IEnumerable<AiringMovie> GetAiringMoviesFromRoom(Room room)
        {
            var airingMovies = new List<AiringMovie>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAiringsFromRoom", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomId", room.Number));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        Id = (int)reader["AiringMovieId"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = new Movie
                        {
                            Title = reader["MovieName"]?.ToString(),
                            Id = (int)reader["MovieId"]
                        },
                        Room = new Room
                        {
                            Number = (int)reader["RoomId"],
                            Type = reader["RoomType"].ToString()
                        }
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }

        public IEnumerable<AiringMovie> GetAiringMoviesFromRoomByDate(Room room, DateTime date)
        {
            var airingMovies = new List<AiringMovie>();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAiringsFromRoomByDate", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RoomId", room.Number));
                cmd.Parameters.Add(new SqlParameter("@Date", date.Date));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        Id = (int)reader["AiringMovieId"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = new Movie
                        {
                            Title = reader["MovieName"]?.ToString(),
                            Id = (int)reader["MovieId"]
                        },
                        Room = new Room
                        {
                            Number = (int)reader["RoomId"],
                            Type = reader["RoomType"].ToString()
                        }
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }
    }
}
