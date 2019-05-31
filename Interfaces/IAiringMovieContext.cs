using System;
using System.Collections.Generic;
using Models;
using Models.Enums;

namespace Interfaces
{
    public interface IAiringMovieContext
    {
        IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie);
        AiringMovie GetAiringById(int id);
        IEnumerable<AiringMovie> GetAiringsByRoomType(string roomType);
        void AddAiring(AiringMovie airingMovie);
        IEnumerable<AiringMovie> GetAiringsFromRoom(Room room);
        IEnumerable<AiringMovie> GetAiringsFromRoomByDate(Room room, DateTime date);
        IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date);
        IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date);
    }
}