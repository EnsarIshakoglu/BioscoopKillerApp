using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;
using Models.Enums;

namespace DAL
{
    public class AiringMovieContext : IAiringMovieContext
    {
        private readonly string _dbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";

        public IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie)
        {
            var airingMovies = new List<AiringMovie>();
            try
            {
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
                            Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"], (int)reader["SeatsPerRow"]),
                            Price = (decimal)reader["Price"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airingMovies;
        }

        public AiringMovie GetAiringById(int id)
        {
            var airingMovie = new AiringMovie();
            try
            {
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
                            Price = (decimal)reader["Price"],
                            Movie = new Movie
                            {
                                Id = (int)reader["MovieId"]
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airingMovie;
        }

        public void DeleteAiring(AiringMovie airing)
        {
            try
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var sqlCommand = new SqlCommand($"DeleteAiring", connection)
                    { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.AddWithValue("@AiringId", airing.Id);

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }
        }

        public IEnumerable<AiringMovie> GetAiringsByRoomType(string roomType)
        {
            var airingMovies = new List<AiringMovie>();
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airingMovies;
        }

        public void AddAiring(AiringMovie airingMovie)
        {
            try
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("AddAiring", connection) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@RoomId", airingMovie.Room.Number));
                    cmd.Parameters.Add(new SqlParameter("@AiringTime", airingMovie.AiringTime));
                    cmd.Parameters.Add(new SqlParameter("@MovieId", airingMovie.Movie.Id));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

        }

        public IEnumerable<AiringMovie> GetAiringsFromRoom(Room room)
        {
            var airingMovies = new List<AiringMovie>();

            try
            {
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airingMovies;
        }

        public IEnumerable<AiringMovie> GetAiringsFromRoomByDate(Room room, DateTime date)
        {
            var airingMovies = new List<AiringMovie>();

            try
            {
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airingMovies;
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date)
        {
            var airings = new List<AiringMovie>();

            try
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var cmd = new SqlCommand("GetAiringsFromMovieByDate", connection) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@MovieId", movie.Id));
                    cmd.Parameters.Add(new SqlParameter("@Date", date.Date));

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        airings.Add(new AiringMovie
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
                throw ex;
            }

            return airings;
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date)
        {
            var airings = new List<AiringMovie>();

            try
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var cmd = new SqlCommand("GetAiringsFromMovieStartingFromDate", connection) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@MovieId", movie.Id));
                    cmd.Parameters.Add(new SqlParameter("@Date", date.Date));

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        airings.Add(new AiringMovie
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return airings;
        }
    }
}
