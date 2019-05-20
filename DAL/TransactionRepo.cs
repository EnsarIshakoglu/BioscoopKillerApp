using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;
using Models;

namespace DAL
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
            _context.AddOccupiedSeats(airingMovie);
        }

        public void SaveReservation(Reservation reservation)
        {
            _context.SaveReservation(reservation);
        }
    }
}
