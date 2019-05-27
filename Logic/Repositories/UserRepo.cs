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

        public bool Login(User user)
        {
            return _context.CheckLogin(user);
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            return _context.GetUserRoles(user);
        }

        public bool CreateAccount(User user)
        {
            return _context.CreateAccount(user);
        }

        public bool IsEmailInUse(User user)
        {
            return _context.IsEmailInUse(user);
        }

        public User GetUserByEmail(string email)
        {
            return _context.GetUserByEmail(email);
        }
    }
}
