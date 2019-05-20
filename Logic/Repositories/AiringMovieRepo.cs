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
            _context.AddAiringMovie(airingMovie, startTimeMovie);
        }
    }
}
