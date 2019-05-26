using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class RoomLogic
    {
        private readonly RoomRepo _repo = new RoomRepo(new RoomContext());

        public IEnumerable<string> GetAllRoomTypes()
        {
            return _repo.GetAllRoomTypes();
        }

        public IEnumerable<Room> GetRoomsByRoomType(string roomType)
        {
            return _repo.GetRoomsByRoomType(roomType);
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            return _repo.GetAiringMoviesByRoomType(roomType);
        }
    }
}
