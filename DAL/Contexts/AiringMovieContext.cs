using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;
using Models.Enums;

namespace DAL.Contexts
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
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"],(int)reader["SeatsPerRow"]),
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
                        Room = new Room((int)reader["RoomNumber"], reader["RoomType"].ToString(), (int)reader["AvailablePlaces"], (int)reader["SeatsPerRow"]),
                        Price = (int)reader["Price"]
                    };
                }

                connection.Close();
            }

            return airingMovie;
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            var airingMovies = new List<AiringMovie>();
            var query =
                $"select Planning.ID as AiringID, AiringTime, [m].[ID] as MovieId, [m].[Name], r.AvailablePlaces, r.ID as RoomNumber, [t].[Name] as RoomType, [t].SeatsPerRow, ([t].RoomPrice + m.MoviePrice) as Price from Planning " +
                $"inner join Room r on r.ID = Planning.RoomID " +
                $"inner join Movie m on m.ID = Planning.MovieID " +
                $"inner join TypeRoom [t] on [t].[ID] = r.TypeRoomID " +
                $"where [t].[Name] = '{roomType}'";

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand(query, connection);

                var reader = sqlCommand.ExecuteReader();

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
                        Price = (int)reader["Price"]
                    });

                    
                }

                connection.Close();
            }

            return airingMovies;
        }

        public void AddAiringMovie(AiringMovie airingMovie, DateTime startTimeMovie)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand =
                    new SqlCommand(
                        $"INSERT INTO dbo.Movie (RoomID, MovieID, AiringTime) VALUES (@RoomID, @MovieID, @AiringTime)",
                        connection);

                sqlCommand.Parameters.AddWithValue("@RoomID", airingMovie.Room.Number);
                sqlCommand.Parameters.AddWithValue("@MovieID", airingMovie.Movie.Id);
                sqlCommand.Parameters.AddWithValue("@AiringTime", startTimeMovie);
                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
