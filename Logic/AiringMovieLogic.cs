using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Models;

namespace Logic
{
    public class AiringMovieLogic
    {
        private readonly AiringMovieRepo _repo = new AiringMovieRepo();

        public IEnumerable<AiringMovie> GetAiringMovies(Movie movie)
        {
            return _repo.GetAiringMovies(movie);
        }
    }
}
