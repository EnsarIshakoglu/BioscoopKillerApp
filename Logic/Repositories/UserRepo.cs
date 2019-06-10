using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Models;

namespace Logic.Repositories
{
    public class UserRepo
    {
        private readonly IUserContext _context;

        public UserRepo(IUserContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            return _context.GetUserRoles(user);
        }

        public bool CreateAccount(User user)
        {
            return _context.CreateAccount(user);
        }

        public bool IsEmailInUse(string email)
        {
            return _context.IsEmailInUse(email);
        }

        public User GetUserByEmail(string email)
        {
            return _context.GetUserByEmail(email);
        }
    }
}
