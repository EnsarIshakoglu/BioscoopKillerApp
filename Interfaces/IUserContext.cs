using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IUserContext
    {
        IEnumerable<string> GetUserRoles(User user);
        bool CreateAccount(User user);
        bool IsEmailInUse(string email);
        User GetUserByEmail(string email);
    }
}