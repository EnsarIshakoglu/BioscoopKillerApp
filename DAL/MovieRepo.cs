using System;
using System.Collections.Generic;
using DAL.Contexts;
using Models;

namespace DAL
{
    public class MovieRepo
    {
        private readonly IMovieContext _movieContext;

        public MovieRepo()
        {
            _movieContext = new MovieContext();
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movieContext.GetAllMovies();
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieContext.GetMovieById(movieId);
        }

        public void AddMovie(Movie movie)
        {
            _movieContext.AddMovie(movie);
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            return _movieContext.CheckIfMovieExists(movie);
        }
    }
}
