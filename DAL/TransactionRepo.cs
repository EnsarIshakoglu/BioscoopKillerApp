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

        public TransactionRepo()
        {
            _context = new TransactionContext();
        }

        public void AddOccupiedSeats(AiringMovie airingMovie)
        {
            _context.AddOccupiedSeats(airingMovie);
        }
    }
}
