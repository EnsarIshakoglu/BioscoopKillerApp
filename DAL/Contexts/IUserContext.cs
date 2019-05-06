using System.Collections.Generic;
using Models;

namespace DAL.Contexts
{
    public interface IUserContext
    {
        bool Login(User user);
        IEnumerable<string> GetUserRoles(User user);
        bool CreateAccount(User user);
        bool IsEmailInUse(User user);
    }
}