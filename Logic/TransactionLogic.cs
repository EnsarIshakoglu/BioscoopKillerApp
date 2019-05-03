using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Models;

namespace Logic
{
    public class TransactionLogic
    {
        private readonly TransactionRepo _transactionRepo = new TransactionRepo();
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic();
        private readonly MovieLogic _movieLogic = new MovieLogic();

        public AiringMovie GetAiringMovieById(int id)
        {
            var airingMovie = _airingMovieLogic.GetAiringMovieById(id);
            _transactionRepo.AddOccupiedSeats(airingMovie);

            return airingMovie;
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieLogic.GetMovieById(movieId);
        }
    }
}
