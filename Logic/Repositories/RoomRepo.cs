using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Models;

namespace Logic.Repositories
{
    public class RoomRepo
    {
        private readonly IRoomContext _context;

        public RoomRepo(IRoomContext context)
        {
            _context = context;
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
