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
    }
}
