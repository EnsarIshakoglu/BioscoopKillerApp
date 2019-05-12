using System.Collections.Generic;

namespace DAL.Contexts
{
    public interface IRoomContext
    {
        IEnumerable<string> GetAllRoomTypes();
    }
}