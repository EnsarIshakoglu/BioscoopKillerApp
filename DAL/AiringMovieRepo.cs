using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;
using Models;
using Models.Enums;

namespace DAL
{
    public class AiringMovieRepo
    {
        private readonly IAiringMovieContext _context;

        public AiringMovieRepo()
        {
            _context = new AiringMovieContext();
        }

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            return _context.GetAiringMoviesFromMovie(movie);
        }

        public AiringMovie GetAiringMovieById(int id)
        {
            return _context.GetAiringMovieById(id);
        }
        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            return _context.GetAiringMoviesByRoomType(roomType);
        }

        public void AddAiringMovie(AiringMovie airingMovie, DateTime startTimeMovie)
        {
            throw new NotImplementedException();
        }
    }
}
