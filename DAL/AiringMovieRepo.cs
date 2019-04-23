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

        public IEnumerable<AiringMovie> GetAiringMovies(Movie movie)
        {
            return _context.GetAiringMovies(movie);
        }
    }
}
