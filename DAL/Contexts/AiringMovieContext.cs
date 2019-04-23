using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DAL.Contexts
{
    public class AiringMovieContext : IAiringMovieContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB; Integrated Security = True";

        public IEnumerable<AiringMovie> GetAiringMovies(Movie movie)
        {
            var airingMovies = new List<AiringMovie>(); //todo give airingmovie standard movie so it doesn't give an exception (this airingmovie needs to be filled with sql data later on)

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"select AiringTime, [m].[Name], r.AvailablePlaces, r.ID as RoomNumber, [t].[Name] as RoomType from Planning " +
                                                $"inner join Room r on r.ID = Planning.RoomID " +
                                                $"inner join Movie m on m.ID = Planning.MovieID " +
                                                $"inner join TypeRoom t on t.ID = r.TypeRoomID " +
                                                $"where m.ID = {movie.Id}", connection);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    airingMovies.Add(new AiringMovie
                    {
                        AiringTime = (DateTime)reader["AiringTime"],
                        Movie = movie,
                        Room = new Room
                        {
                            RoomNumber = (int)reader["RoomNumber"],
                            RoomType = reader["RoomType"]?.ToString(),
                            SeatCount = (int)reader["AvailablePlaces"]
                        }
                    });
                }

                connection.Close();
            }

            return airingMovies;
        }
    }
}
