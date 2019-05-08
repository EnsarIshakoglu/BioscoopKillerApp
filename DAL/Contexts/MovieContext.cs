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
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";
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
                    movie.Actors = apiDataMovie.Actors;
                    movie.Genre = apiDataMovie.Genre;
                }
            }

        }

        public Movie GetMovieById(int movieId)
        {
            Movie movie = new Movie();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand($"SELECT * FROM dbo.Movie WHERE Movie.ID = {movieId}", connection);
                var reader = sqlCommand.ExecuteReader();

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

            AddAPIData(movie).Wait();


            return movie;
        }

        public void AddMovie(Movie movie)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand =
                    new SqlCommand(
                        $"INSERT INTO dbo.Movie (Name, PublishedYear, MoviePrice) VALUES (@Name, @PublishedYear, @MoviePrice)",
                        connection);

                sqlCommand.Parameters.AddWithValue("@Name", movie.Title);
                sqlCommand.Parameters.AddWithValue("@PublishedYear", movie.PublishedYear);
                sqlCommand.Parameters.AddWithValue("@MoviePrice", movie.MoviePrice);
                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
