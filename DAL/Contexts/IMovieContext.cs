using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL.Contexts
{
    public interface IMovieContext
    {
        IEnumerable<Movie> GetAllMovies();
        Movie GetMovieById(int movieId);
        void AddMovie(Movie movie);
        bool CheckIfMovieExists(Movie movie);
    }
}
