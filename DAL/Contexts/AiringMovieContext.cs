using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DAL.Contexts
{
    public class AiringMovieContext : IAiringMovieContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public IEnumerable<AiringMovie> GetAiringMovies(Movie movie)
        {
            var airingMovies = new List<AiringMovie>(); //todo give airingmovie standard movie so it doesn't give an exception (this airingmovie needs to be filled with sql data later on)

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"select Planning.ID as AiringID, AiringTime, [m].[Name], r.AvailablePlaces, r.ID as RoomNumber, [t].[Name] as RoomType, t.SeatsPerRow, (t.RoomPrice + m.MoviePrice) as Price from Planning " +
                                                $"inner join Room r on r.ID = Planning.RoomID " +
                                                $"inner join Movie m on m.ID = Planning.MovieID " +
                                                $"inner join TypeRoom t on t.ID = r.TypeRoomID " +
                                                $"where m.ID = {movie.Id}", connection);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        Id = (int)reader["AiringID"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = movie,
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"]?.ToString(), (int)reader["AvailablePlaces"],(int)reader["SeatsPerRow"]),
                        /*{
                            SeatCount = (int)reader["AvailablePlaces"],
                            SeatsPerRow = (int)reader["SeatsPerRow"],
                            RoomNumber = (int)reader["RoomNumber"],
                            RoomType = reader["RoomType"]?.ToString()
                        }*/
                        Price = (int)reader["Price"]
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }

        public AiringMovie GetAiringMovieById(int id)
        {
            AiringMovie airingMovie = new AiringMovie();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"select Planning.ID as AiringID, AiringTime, [m].[Name], r.AvailablePlaces, r.ID as RoomNumber, [t].[Name] as RoomType, t.SeatsPerRow, (t.RoomPrice + m.MoviePrice) as Price from Planning " +
                                                $"inner join Room r on r.ID = Planning.RoomID " +
                                                $"inner join Movie m on m.ID = Planning.MovieID " +
                                                $"inner join TypeRoom t on t.ID = r.TypeRoomID " +
                                                $"where Planning.ID = {id}", connection);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                     airingMovie = new AiringMovie
                    {
                        Id = (int)reader["AiringID"],
                        AiringTime = (DateTime)reader["AiringTime"],
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"]?.ToString(), (int)reader["AvailablePlaces"], (int)reader["SeatsPerRow"]),
                        Price = (int)reader["Price"]
                    };
                }

                connection.Close();
            }

            return airingMovie;
        }
    }
}
