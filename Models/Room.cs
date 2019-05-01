using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Room
    {
        public Room()
        {
            
        }
        public Room(int roomNumber, string roomType, int seatCount, int seatsPerRow)
        {
            Seats = new List<Seat>();
            
            RoomNumber = roomNumber;
            RoomType = roomType;
            SeatCount = seatCount;
            SeatsPerRow = seatsPerRow;

            for (int x = 0; x < SeatCount; x++)
            {
                Seats.Add(new Seat());
            }

            int a = 5;
        }

        public List<Seat> Seats { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int SeatCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
