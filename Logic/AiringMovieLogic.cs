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

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            return _repo.GetAiringMoviesFromMovie(movie);
        }

        public IEnumerable<AiringMovie> GetAllAiringMovies(Movie movie)
        {
            return _repo.GetAllAiringMovies(movie);
        }

        public AiringMovie GetAiringMovieById(int id)
        {
            return _repo.GetAiringMovieById(id);
        }

        public void AddAiringMovie(AiringMovie airingMovie)
        {
            var airingMovies = 
        }
    }
}
