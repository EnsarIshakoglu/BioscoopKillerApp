using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.MockContexts;
using Interfaces;
using Interfaces.ContextInterfaces;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class RoomLogic
    {
        public RoomLogic(IRoomContext context)
        {
            _repo = new RoomRepo(context);
        }
        private readonly RoomRepo _repo;

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
