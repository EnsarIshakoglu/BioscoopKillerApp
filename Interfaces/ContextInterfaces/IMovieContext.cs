using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Interfaces.ContextInterfaces
{
    public interface IMovieContext
    {
        IEnumerable<Movie> GetAllMovies();
        Movie GetMovieById(int movieId);
        void AddMovie(Movie movie);
        IEnumerable<Movie> GetMoviesByGenre(string category);
        IEnumerable<string> GetAllGenres();
        IEnumerable<Movie> GetMoviesBySearchParam(string searchParam);
        void DeleteMovie(Movie movie);
    }
}
