using System.Collections.Generic;
using Models;

namespace DAL.Contexts
{
    public interface IAiringMovieContext
    {
        IEnumerable<AiringMovie> GetAiringMovies(Movie movie);
    }
}