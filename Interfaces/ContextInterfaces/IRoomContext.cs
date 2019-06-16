using System.Collections.Generic;
using Models;

namespace Interfaces.ContextInterfaces
{
    public interface IRoomContext
    {
        IEnumerable<string> GetAllRoomTypes();
        IEnumerable<Room> GetRoomsByRoomType(string roomType);
        IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType);
    }
}