﻿using System.Collections.Generic;
using Models;

namespace DAL.Contexts
{
    public interface IRoomContext
    {
        IEnumerable<string> GetAllRoomTypes();
        IEnumerable<int> GetRoomIdsByRoomType(string roomType);
        IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType);
    }
}