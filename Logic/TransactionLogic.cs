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
            return _airingMovieLogic.GetAiringMovieById(id);
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieLogic.GetMovieById(movieId);
        }
    }
}
