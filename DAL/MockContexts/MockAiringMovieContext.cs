using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using Models;

namespace DAL.MockContexts
{
    public class MockAiringMovieContext : IAiringMovieContext
    {
        private IEnumerable<AiringMovie> _airings = new List<AiringMovie>
        {
            new AiringMovie
            {
                Id = 1,
                Movie = new Movie
                {
                    Id = 1,
                    Title = "Finding Nemo",
                    Runtime = "120 min"
                },
                Room = new Room
                {
                    Number = 10,
                    Type = "IMAX 2D"
                },
                AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0)
            },
            new AiringMovie
            {
                Id = 2,
                Movie = new Movie
                {
                    Id = 2,
                    Title = "X-men",
                    Runtime = "130 min"
                },
                Room = new Room
                {
                    Number = 11,
                    Type = "2D"
                },
                AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15, 0, 0)
            },
            new AiringMovie
            {
                Id = 3,
                Movie = new Movie
                {
                    Id = 3,
                    Title = "Avengers",
                    Runtime = "120 min"
                },
                Room = new Room
                {
                    Number = 14,
                    Type = "IMAX 3D"
                },
                AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18, 10, 0)
            },
            new AiringMovie
            {
                Id = 5,
                Movie = new Movie
                {
                    Id = 5,
                    Title = "Avengers",
                    Runtime = "120 min"
                },
                Room = new Room
                {
                    Number = 14,
                    Type = "IMAX 3D"
                },
                AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 21, 10, 0)
            },
            new AiringMovie
            {
                Id = 4,
                Movie = new Movie
                {
                    Id = 4,
                    Title = "Avengers",
                    Runtime = "130 min"
                },
                Room = new Room
                {
                    Number = 12,
                    Type = "3D"
                },
                AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11, 00, 0)
            }
        };
        public IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie)
        {
            return _airings.Where(a => a.Movie.Title.Equals(movie.Title));
        }

        public AiringMovie GetAiringById(int id)
        {
            return _airings.First(a => a.Id.Equals(id));
        }

        public IEnumerable<AiringMovie> GetAiringsByRoomType(string roomType)
        {
            return _airings.Where(a => a.Room.Type.Equals(roomType));
        }
        /// <summary>
        /// DO NUT USE! NOT IMPLEMENTED!
        /// </summary>
        /// <param name="airingMovie"></param>
        public void AddAiring(AiringMovie airingMovie)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AiringMovie> GetAiringsFromRoom(Room room)
        {
            return _airings.Where(a => a.Room.Number.Equals(room.Number));
        }

        public IEnumerable<AiringMovie> GetAiringsFromRoomByDate(Room room, DateTime date)
        {
            return _airings.Where(a => a.Room.Number.Equals(room.Number) && a.AiringTime.Date.Equals(date.Date));
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date)
        {
            return _airings.Where(a => a.Movie.Title.Equals(movie.Title) && a.AiringTime.Date.Equals(date.Date));
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date)
        {
            return _airings.Where(a => a.AiringTime.Date.Equals(date));
        }

        /// <summary>
        /// DO NUT USE! NOT IMPLEMENTED
        /// </summary>
        /// <param name="airing"></param>
        public void DeleteAiring(AiringMovie airing)
        {
            throw new NotImplementedException();
        }
    }
}
