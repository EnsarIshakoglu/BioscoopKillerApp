using Models;

namespace DAL.Contexts
{
    public interface IUserContext
    {
        bool Login(User user);
    }
}