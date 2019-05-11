using System.Collections.Generic;
using Models;

namespace DAL.Contexts
{
    public interface IAiringMovieContext
    {
        IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie);
        AiringMovie GetAiringMovieById(int id);
        IEnumerable<AiringMovie> GetAllAiringMovies(Movie movie);
    }
}