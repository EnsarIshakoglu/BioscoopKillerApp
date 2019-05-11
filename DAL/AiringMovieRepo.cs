using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;
using Models;

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

        public IEnumerable<AiringMovie> GetAllAiringMovies(Movie movie)
        {
            return _context.GetAllAiringMovies(movie);
        }
    }
}
