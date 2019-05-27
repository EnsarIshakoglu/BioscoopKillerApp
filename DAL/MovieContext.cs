using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Interfaces;
using Models;

namespace DAL
{
    public class MovieContext : IMovieContext
    {
        private const string DbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog=CinemaDB_2;Integrated Security=True";
        private readonly ApiHelper _apiHelper = new ApiHelper();

        public IEnumerable<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAllMovies", connection) { CommandType = CommandType.StoredProcedure };
                
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var movie = new Movie
                    {
                        Id = (int)reader["ID"],
                        Title = reader["Name"]?.ToString(),
                        PublishedYear = (int)reader["PublishedYear"]
                    };

                    movies.Add(movie);
                }

                connection.Close();
            }

            foreach (var movie in movies)
            {
                _apiHelper.AddApiDataToMovie(movie).Wait();
            }

            return movies;
        }

        

        public Movie GetMovieById(int movieId)
        {
            Movie movie = new Movie();

            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetMovieById", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@MovieId", movieId));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    movie = new Movie
                    {
                        Id = (int)reader["ID"],
                        Title = reader["Name"]?.ToString(),
                        PublishedYear = (int)reader["PublishedYear"]
                    };
                }

                connection.Close();
            }

            _apiHelper.AddApiDataToMovie(movie).Wait();

            return movie;
        }

        public void AddMovie(Movie movie)
        {
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"AddMovie", connection) { CommandType = CommandType.StoredProcedure };

                sqlCommand.Parameters.AddWithValue("@Name", movie.Title);
                sqlCommand.Parameters.AddWithValue("@PublishedYear", movie.PublishedYear);
                sqlCommand.Parameters.AddWithValue("@MoviePrice", movie.MoviePrice);
                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            var movieExists = false;

            _apiHelper.AddApiDataToMovie(movie).Wait();

            if (movie.Poster != null)
            {
                movieExists = true;
            }

            return movieExists;
        }
    }
}
