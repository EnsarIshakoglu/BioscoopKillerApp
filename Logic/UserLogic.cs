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
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();

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

        public User GetUserByEmail(string email)
        {
            return _userRepo.GetUserByEmail(email);
        }

        public User InitUser(User user)
        {
            var email = user.Email;
            user = GetUserByEmail(email);

            var roles = GetUserRoles(user);

            foreach (var role in roles)
            {
                Enum.TryParse(role, out Roles enumRole);
                user.Roles.Add(enumRole);
            }

            return user;
        }

        public void SaveReview(Review review)
        {
            _reviewLogic.SaveReview(review);
        }
    }
}
