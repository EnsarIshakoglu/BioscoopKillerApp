using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;

namespace DAL
{
    public class TransactionRepo
    {
        private readonly ITransactionContext _context;

        public TransactionRepo()
        {
            _context = new TransactionContext();
        }
    }
}
