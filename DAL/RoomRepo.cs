using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;
using Models;

namespace DAL
{
    public class RoomRepo
    {
        private readonly IRoomContext _context;

        public RoomRepo()
        {
            _context = new RoomContext();
        }

        public IEnumerable<string> GetAllRoomTypes()
        {
            return _context.GetAllRoomTypes();
        }

        public IEnumerable<int> GetRoomIdsByRoomType(string roomType)
        {
            return _context.GetRoomIdsByRoomType(roomType);
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            return _context.GetAiringMoviesByRoomType(roomType);
        }
    }
}
