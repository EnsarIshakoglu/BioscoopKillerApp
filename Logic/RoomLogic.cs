using System;
using System.Collections.Generic;
using System.Text;
using DAL;

namespace Logic
{
    public class RoomLogic
    {
        private readonly RoomRepo _repo = new RoomRepo();

        public IEnumerable<string> GetAllRoomTypes()
        {
            return _repo.GetAllRoomTypes();
        }

        public IEnumerable<int> GetRoomIdsByRoomType(string roomType)
        {
            return _repo.GetRoomIdsByRoomType(roomType);
        }
    }
}
