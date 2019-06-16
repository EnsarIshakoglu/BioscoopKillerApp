using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace DAL
{
    public class MovieContext : IMovieContext
    {
        private const string DbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";

        public IEnumerable<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return movies;
        }



        public Movie GetMovieById(int movieId)
        {
            Movie movie = new Movie();

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return movie;
        }

        public void AddMovie(Movie movie)
        {
            try
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
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            AddCategoriesToMovie(movie);
        }

        private void AddCategoriesToMovie(Movie movie)
        {
            var categories = movie.Genre.Replace(" ", string.Empty).Split(',').ToList();

            foreach (var category in categories)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DbConnectionString))
                    {
                        connection.Open();

                        var sqlCommand = new SqlCommand($"AddCategoryToMovie", connection) { CommandType = CommandType.StoredProcedure };

                        sqlCommand.Parameters.AddWithValue("@MovieTitle", movie.Title);
                        sqlCommand.Parameters.AddWithValue("@Category", category);

                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //File.AppendAllText(, message);
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            var movies = new List<Movie>();

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return movies;
        }

        public IEnumerable<string> GetAllGenres()
        {
            var genres = new List<string>();

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }

            return genres;
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            var movies = new List<Movie>();

            try
            {
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
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }
            
            return movies;
        }

        public void DeleteMovie(Movie movie)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString))
                {
                    connection.Open();

                    var sqlCommand = new SqlCommand($"DeleteMovie", connection) { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.AddWithValue("@MovieId", movie.Id);

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
