using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;
using Models.Enums;

namespace DAL.MockContexts
{
    public class MockUserContext : IUserContext
    {
        private readonly IEnumerable<User> _users = new List<User>
        {
            new User
            {
                Email = "test@test.com",
                Id = 1,
                Name = "Test",
                Password = "Test123",
                Roles = new List<Roles>
                {
                    Roles.AccountHolder,
                    Roles.Admin
                },
                SurName = "Unit"
             },
            new User
            {
                Email = "test2@test2.com",
                Id = 2,
                Name = "Test2",
                Password = "Test123",
                Roles = new List<Roles>
                {
                    Roles.AccountHolder
                },
                SurName = "Unit2"
            }
        };
        public IEnumerable<string> GetUserRoles(User model)
        {
            var user = _users.First(u => u.Id.Equals(model.Id));
            var roles = new List<string>();

            foreach (var role in user.Roles)
            {
                roles.Add(role.ToString());
            }

            return roles;
        }

        public bool CreateAccount(User user)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailInUse(string email)
        {
            return _users.Any(u => u.Email.Equals(email));
        }

        public User GetUserByEmail(string email)
        {
            return _users.First(u => u.Email.Equals(email));
        }
    }
}
