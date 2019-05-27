﻿using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IUserContext
    {
        bool CheckLogin(User user);
        IEnumerable<string> GetUserRoles(User user);
        bool CreateAccount(User user);
        bool IsEmailInUse(User user);
        int GetUserId(User user);
    }
}