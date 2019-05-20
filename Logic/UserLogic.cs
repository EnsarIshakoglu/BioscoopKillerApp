using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Logic.Repositories;
using Models;
using Models.Enums;

namespace Logic
{
    public class UserLogic
    {
        private readonly UserRepo _userRepo = new UserRepo(new UserContext());

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

        public void InitUser(User user)
        {
            user.Id = GetUserId(user);

            var roles = GetUserRoles(user);

            foreach (var role in roles)
            {
                Enum.TryParse(role, out Roles enumRole);
                user.Roles.Add(enumRole);
            }
        }
    }
}
