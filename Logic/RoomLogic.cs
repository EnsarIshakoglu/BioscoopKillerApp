﻿using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.Contexts;
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

        public IEnumerable<int> GetRoomIdsByRoomType(string roomType)
        {
            return _repo.GetRoomIdsByRoomType(roomType);
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            return _repo.GetAiringMoviesByRoomType(roomType);
        }
    }
}
