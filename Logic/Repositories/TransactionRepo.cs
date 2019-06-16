using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace Logic.Repositories
{
    public class TransactionRepo
    {
        private readonly ITransactionContext _context;

        public TransactionRepo(ITransactionContext context)
        {
            _context = context;
        }

        public void AddOccupiedSeats(AiringMovie airingMovie)
        {
            _context.GetOccupiedSeats(airingMovie);
        }

        public void SaveReservation(Reservation reservation)
        {
            _context.SaveReservation(reservation);
        }
    }
}
