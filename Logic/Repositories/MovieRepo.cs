using System;
using System.Collections.Generic;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace Logic.Repositories
{
    public class MovieRepo
    {
        private readonly IMovieContext _movieContext;
        private readonly IApiHelper _apiHelper;

        public MovieRepo(IMovieContext context, IApiHelper apiHelper)
        {
            _movieContext = context;
            _apiHelper = apiHelper;
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            var movies = _movieContext.GetAllMovies();
            AddApiData(movies);

            return movies;
        }

        public void DeleteMovie(Movie movie)
        {
            _movieContext.DeleteMovie(movie);
        }

        public Movie GetMovieById(int movieId)
        {
            var movie = _movieContext.GetMovieById(movieId);
            AddApiData(movie);

            return movie;
        }

        public void AddMovie(Movie movie)
        {
            _movieContext.AddMovie(movie);
        }

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            var movies = _movieContext.GetMoviesByGenre(category);
            AddApiData(movies);

            return movies;
        }

        public IEnumerable<string> GetAllGenres()
        {
            return _movieContext.GetAllGenres();
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            var movies = _movieContext.GetMoviesBySearchParam(searchParam);
            AddApiData(movies);

            return movies;
        }

        public void AddApiData(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                _apiHelper.AddApiDataToMovie(movie).Wait();
            }
        }

        public void AddApiData(Movie movie)
        {
            _apiHelper.AddApiDataToMovie(movie).Wait();
        }


    }
}
