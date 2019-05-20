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
        public Room(int number, string type, int seatCount, int seatsPerRow)
        {
            Seats = new List<Seat>();
            
            Number = number;
            Type = type;
            SeatCount = seatCount;
            SeatsPerRow = seatsPerRow;

            for (int x = 0; x < SeatCount; x++)
            {
                Seats.Add(new Seat
                {
                    SeatNumber = x+1
                });
            }
        }

        public List<Seat> Seats { get; set; }
        public int? Number { get; set; }
        public string Type { get; set; }
        public int SeatCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
