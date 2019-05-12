using System;
using System.Collections.Generic;
using System.Text;
using DAL.Contexts;

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
    }
}
