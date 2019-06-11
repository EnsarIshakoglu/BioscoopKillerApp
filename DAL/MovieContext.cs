using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        private const string DbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";
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
            }

            AddApiData(movies);

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
            }

            AddApiData(movie);

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

            AddCategoriesToMovie(movie);
        }

        private void AddCategoriesToMovie(Movie movie)
        {
            var categories = movie.Genre.Replace(" ", string.Empty).Split(',').ToList();

            foreach (var category in categories)
                using (SqlConnection connection = new SqlConnection(DbConnectionString))
                {
                    connection.Open();

                    var sqlCommand = new SqlCommand($"AddCategoryToMovie", connection) { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.AddWithValue("@MovieTitle", movie.Title);
                    sqlCommand.Parameters.AddWithValue("@Category", category);

                    sqlCommand.ExecuteNonQuery();
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

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetMoviesByGenre", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@Category", category));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    movies.Add(new Movie
                    {
                        Id = (int)reader["ID"],
                        Title = reader["Name"]?.ToString(),
                        PublishedYear = (int)reader["PublishedYear"]
                    });
                }
            }

            AddApiData(movies);

            return movies;
        }

        public IEnumerable<string> GetAllGenres()
        {
            var genres = new List<string>();

            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetAllGenres", connection) { CommandType = CommandType.StoredProcedure };

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    genres.Add(reader["Name"]?.ToString());
                }
            }

            return genres;
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetMoviesBySearchParam", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@SearchParam", searchParam));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    movies.Add(new Movie
                    {
                        Id = (int)reader["ID"],
                        Title = reader["Name"]?.ToString(),
                        PublishedYear = (int)reader["PublishedYear"]
                    });
                }
            }

            AddApiData(movies);

            return movies;
        }

        public void DeleteMovie(Movie movie)
        {
            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"DeleteMovie", connection) { CommandType = CommandType.StoredProcedure };

                sqlCommand.Parameters.AddWithValue("@MovieId", movie.Id);

                sqlCommand.ExecuteNonQuery();
            }
        }

        private void AddApiData(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                _apiHelper.AddApiDataToMovie(movie).Wait();
            }
        }

        private void AddApiData(Movie movie)
        {
            _apiHelper.AddApiDataToMovie(movie).Wait();
        }
    }
}
