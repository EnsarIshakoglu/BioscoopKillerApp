using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Interfaces;
using Interfaces.ContextInterfaces;
using Logic.Repositories;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Enums;

namespace Logic
{
    public class UserLogic
    {
        private readonly UserRepo _userRepo;
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();

        public UserLogic(IUserContext context)
        {
            _userRepo = new UserRepo(context);
        }

        public IEnumerable<string> GetUserRoles(User user)
        {
            return _userRepo.GetUserRoles(user);
        }

        public bool CreateAccount(User user)
        {
            return _userRepo.CreateAccount(user);
        }

        public bool IsEmailInUse(string email)
        {
            return _userRepo.IsEmailInUse(email);
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
