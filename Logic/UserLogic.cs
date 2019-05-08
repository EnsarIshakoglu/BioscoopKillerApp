using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Models;

namespace Logic
{
    public class UserLogic
    {
        private readonly UserRepo _userRepo = new UserRepo();

        public bool Login(User user)
        {
            return _userRepo.Login(user);
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            return _userRepo.GetUserRoles(user);
        }

        public bool CreateAccount(User user)
        {
            return _userRepo.CreateAccount(user);
        }

        public bool IsEmailInUse(User user)
        {
            return _userRepo.IsEmailInUse(user);
        }

        public int GetUserId(User user)
        {
            return _userRepo.GetUserId(user);
        }
    }
}
