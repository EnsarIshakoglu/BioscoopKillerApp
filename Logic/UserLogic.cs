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
    }
}
