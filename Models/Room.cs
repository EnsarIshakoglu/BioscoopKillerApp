using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;

namespace Models
{
    public class Room
    {
        public Room()
        {
            
        }
        public Room(int roomNumber, RoomTypes roomType, int seatCount, int seatsPerRow)
        {
            Seats = new List<Seat>();
            
            RoomNumber = roomNumber;
            RoomType = roomType;
            SeatCount = seatCount;
            SeatsPerRow = seatsPerRow;

            for (int x = 0; x < SeatCount; x++)
            {
                Seats.Add(new Seat
                {
                    SeatNumber = x+1
                });
            }

            int a = 5;
        }

        public List<Seat> Seats { get; set; }
        public int RoomNumber { get; set; }
        public RoomTypes RoomType { get; set; }
        public int SeatCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
