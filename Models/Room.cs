using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Room
    {
        public Room()
        {
            Seats = new List<Seat>();
        }
        public List<Seat> Seats { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int SeatCount { get; set; }
    }
}
