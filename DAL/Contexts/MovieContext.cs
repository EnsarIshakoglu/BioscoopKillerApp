using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL.Contexts
{
    class MovieContext : IMovieContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB; Integrated Security = True";
        private readonly string apiKey = "883e4889";

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
                        Title = reader["Name"]?.ToString(),
                        PublishedYear = (int)reader["PublishedYear"]
                    };

                    movies.Add(movie);
                }

                connection.Close();
            }

            AddCategories(movies);
            AddCast(movies);

            foreach (var movie in movies)
            {
                AddAPIData(movie).Wait();
            }
            

            return movies;
        }

        private async Task AddAPIData(Movie movie)
        {
                string movieTitleForApi = movie.Title.Replace(" ", "%3A");
                string url = $"http://www.omdbapi.com/?apikey={ apiKey}&t={ movieTitleForApi}&y={ movie.PublishedYear}";

                HttpClient client = new HttpClient();

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Movie apiDataMovie = await response.Content.ReadAsAsync<Movie>();
                        movie.Poster = apiDataMovie.Poster;
                        movie.Plot = apiDataMovie.Plot;
                        movie.Title = apiDataMovie.Title;
                        movie.Runtime = apiDataMovie.Runtime;
                    }
                }
            
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
                        $"From Movie_Category\r\nInner join Movie on Movie_Category.MovieID = Movie.ID\r\ninner join Category on Movie_Category.CategoryID = Category.ID where Movie.ID = '{movie.Id}'",
                        connection);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movie.Genres.Add(reader["Category"].ToString());
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
                            $"select distinct Movie.ID, Movie.Name as Name, Actor.Name as Actor\r\n" +
                            $"From Movie_Actor\r\nInner join Movie on Movie_Actor.MovieID = Movie.ID\r\ninner join Actor on Movie_Actor.ActorID = Actor.ID \r\nwhere Movie.ID = {movie.Id}",
                            connection);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movie.Cast.Add(reader["Actor"].ToString());
                        }
                    }
                }

                connection.Close();
            }

        }
    }
}
