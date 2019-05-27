using System;
using System.Collections.Generic;
using Models;
using Models.Enums;

namespace Interfaces
{
    public interface IAiringMovieContext
    {
        IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie);
        AiringMovie GetAiringMovieById(int id);
        IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType);
        void AddAiringMovie(AiringMovie airingMovie);
        IEnumerable<AiringMovie> GetAiringMoviesFromRoom(Room room);
        IEnumerable<AiringMovie> GetAiringMoviesFromRoomByDate(Room room, DateTime date);
    }
}