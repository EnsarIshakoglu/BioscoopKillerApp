using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;
using Models;

namespace DAL
{
    public class UserRepo
    {
        private readonly IUserContext _context;

        public UserRepo()
        {
            _context = new UserContext();
        }

        public bool Login(User user)
        {
            return _context.Login(user);
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

        public int GetUserId(User user)
        {
            return _context.GetUserId(user);
        }
    }
}
