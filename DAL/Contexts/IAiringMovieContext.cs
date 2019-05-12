using System;
using System.Collections.Generic;
using Models;
using Models.Enums;

namespace DAL.Contexts
{
    public interface IAiringMovieContext
    {
        IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie);
        AiringMovie GetAiringMovieById(int id);
        IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType);
        void AddAiringMovie(AiringMovie airingMovie, DateTime startTimeMovie);
    }
}