using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.MockContexts;
using Interfaces;
using Interfaces.ContextInterfaces;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class TransactionLogic
    {
        public TransactionLogic(ITransactionContext context, IAiringMovieContext airingMovieContext, IMovieContext movieContext, IRoomContext roomContext, IApiHelper apiHelper)
        {
            _transactionRepo = new TransactionRepo(context);
            _airingMovieLogic = new AiringMovieLogic(airingMovieContext, roomContext);
            _movieLogic = new MovieLogic(movieContext, roomContext, apiHelper, airingMovieContext);
        }

        private readonly TransactionRepo _transactionRepo;
        private readonly AiringMovieLogic _airingMovieLogic;
        private readonly MovieLogic _movieLogic;

        public AiringMovie GetAiringMovieById(int id)
        {
            var airing = _airingMovieLogic.GetAiringMovieById(id);
            _transactionRepo.AddOccupiedSeats(airing);
            airing.Movie = _movieLogic.GetMovieById(airing.Movie.Id);

            return airing;
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieLogic.GetMovieById(movieId);
        }

        public void SaveReservation(Reservation reservation)
        {
            _transactionRepo.SaveReservation(reservation);
        }
    }
}
