using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Models;
using Models.Enums;

namespace Logic.Repositories
{
    public class AiringMovieRepo
    {
        private readonly IAiringMovieContext _context;

        public AiringMovieRepo(IAiringMovieContext context)
        {
            _context = context;
        }

        public void DeleteAiring(AiringMovie airing)
        {
            _context.DeleteAiring(airing);
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie)
        {
            return _context.GetAiringsFromMovie(movie);
        }

        public AiringMovie GetAiringById(int id)
        {
            return _context.GetAiringById(id);
        }
        public IEnumerable<AiringMovie> GetAiringsByRoomType(string roomType)
        {
            return _context.GetAiringsByRoomType(roomType);
        }

        public void AddAiring(AiringMovie airingMovie)
        {
            _context.AddAiring(airingMovie);
        }

        public IEnumerable<AiringMovie> GetAiringsFromRoom(Room room)
        {
            return _context.GetAiringsFromRoom(room);
        }

        public IEnumerable<AiringMovie> GetAiringsFromRoomByDate(Room room, DateTime date)
        {
            return _context.GetAiringsFromRoomByDate(room, date);
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date)
        {
            return _context.GetAiringsFromMovieByDate(movie, date);
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date)
        {
            return _context.GetAiringsFromMovieStartingFromDate(movie, date);
        }
    }
}
