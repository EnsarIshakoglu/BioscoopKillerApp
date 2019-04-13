using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DAL.Contexts
{
    class MovieContext : IMovieContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = MVCTestVoorKillerApp; Integrated Security = True";

        public IEnumerable<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"SELECT * FROM dbo.Movie", connection);
                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    var movie = new Movie
                    {
                        Id = (int)reader["ID"],
                        Title = reader["Name"]?.ToString()
                    };

                    movies.Add(movie);
                }

                connection.Close();
            }

            AddCategories(movies);
            AddCast(movies);

            return movies;
        }

        private void AddCategories(List<Movie> movies)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                foreach (var movie in movies)
                {
                    var sqlCommand = new SqlCommand(
                        $"select distinct Movie.ID, Movie.Name as Name, Category.Name as Category\r\n" +
                        $"From MovieCategoryTable\r\nInner join Movie on MovieCategoryTable.MovieID = Movie.ID\r\ninner join Category on MovieCategoryTable.CategoryID = Category.ID where Movie.ID = '{movie.Id}'",
                        connection);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movie.Genre.Add(reader["Category"].ToString());
                        }
                    }
                }

                connection.Close();
            }

        }

        private void AddCast(List<Movie> movies)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                foreach (var movie in movies)
                {
                    var sqlCommand =
                        new SqlCommand(
                            $"select Movie.ID as ID, Actor.Name as Actor\r\nFrom Actor\r\nInner join Movie on Actor.MovieID = Movie.ID\r\nwhere Movie.ID = '{movie.Id}'",
                            connection);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movie.Actors.Add(reader["Actor"].ToString());
                        }
                    }
                }

                connection.Close();
            }

        }
    }
}
