using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.MockContexts;
using Interfaces;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class TransactionLogic
    {
        private readonly TransactionRepo _transactionRepo =  new TransactionRepo(new TransactionContext());
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic(new AiringMovieContext());
        private readonly MovieLogic _movieLogic = new MovieLogic(new MovieContext());

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
