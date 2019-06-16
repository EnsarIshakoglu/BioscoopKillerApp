using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace DAL.MockContexts
{
    public class MockRoomContext : IRoomContext
    {
        private IEnumerable<Room> _rooms = new List<Room>
        {
            new Room(1, "3D", 320, 20),
            new Room(2, "IMAX 3D", 320, 20),
            new Room(3, "2D", 320, 20),
            new Room(4, "IMAX 2D", 320, 20),
            new Room(5, "4DX", 300, 20),
            new Room(6, "3D", 320, 20),
            new Room(7, "IMAX 3D", 320, 20),
            new Room(8, "2D", 320, 20),
            new Room(9, "IMAX 2D", 320, 20),
            new Room(10, "4DX", 300, 20),
            new Room(11, "3D", 320, 20),
            new Room(12, "IMAX 3D", 320, 20),
            new Room(13, "2D", 320, 20),
            new Room(14, "IMAX 2D", 320, 20),
            new Room(15, "4DX", 300, 20),
            new Room(16, "3D", 320, 20),
            new Room(17, "IMAX 3D", 320, 20),
            new Room(18, "2D", 320, 20),
            new Room(19, "IMAX 2D", 320, 20),
            new Room(20, "4DX", 300, 20)
        };
        public IEnumerable<string> GetAllRoomTypes()
        {
            return _rooms.GroupBy(r => r.Type).Select(r => r.Key);
        }

        public IEnumerable<Room> GetRoomsByRoomType(string roomType)
        {
            return _rooms.Where(r => r.Type.Equals(roomType));
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            throw new NotImplementedException();
        }
    }
}
