using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL;
using Interfaces;
using Logic.Repositories;
using Models;
using Models.Enums;

namespace Logic
{
    public class AiringMovieLogic
    {
        public AiringMovieLogic(IAiringMovieContext context)
        {
           _repo = new AiringMovieRepo(context);
        }

        private readonly AiringMovieRepo _repo;

        private readonly RoomLogic _roomLogic = new RoomLogic();

        public IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie)
        {
            return _repo.GetAiringsFromMovie(movie);
        }
        public AiringMovie GetAiringMovieById(int id)
        {
            return _repo.GetAiringById(id);
        }
        public void DeleteAiring(AiringMovie airing)
        {
            _repo.DeleteAiring(airing);
        }
        public IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date)
        {
            return _repo.GetAiringsFromMovieStartingFromDate(movie, date);
        }
        public IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date)
        {
            return _repo.GetAiringsFromMovieByDate(movie, date);
        }

        public bool TryToAddAiring(Movie movie, DateTime date, string roomType)
        {
            var airing = FindRoomForMovie(movie, date, roomType);

            if (airing != null)
            {
                AddAiringMovie(airing, date);
                return true;
            }

            return false;
        }
        private AiringMovie FindRoomForMovie(Movie movie, DateTime date, string roomType)
        {
            var rooms = _roomLogic.GetRoomsByRoomType(roomType);
            var roomDoesNotHavePlace = new DateTime(1990, 12, 12, 12, 12, 12);

            foreach (var room in rooms)
            {
                var airingsInRoom = GetAiringsFromRoomByDate(room, date);

                if (airingsInRoom.Any())
                {
                    var startTimeAiring = TryToGetAvailableTime(movie, airingsInRoom, date);

                    if (startTimeAiring != roomDoesNotHavePlace)
                    {
                        return new AiringMovie
                        {
                            Room = room,
                            AiringTime = startTimeAiring,
                            Movie = movie
                        };
                    }
                }
                else
                {
                    return FirstAiringOfTheDay(movie, date, room);
                }
            }

            return null;
        }

        private AiringMovie FirstAiringOfTheDay(Movie movie, DateTime date, Room room)
        {
            var earliestAiringTime = new DateTime(date.Year, date.Month, date.Day, 11, 0, 0);
            var airing = new AiringMovie
            {
                AiringTime = earliestAiringTime,
                Movie = movie,
                Room = room
            };

            return airing;
        }

        private IEnumerable<AiringMovie> GetAiringsFromRoomByDate(Room room, DateTime date)
        {
            var airingMoviesFromRoomByDate = _repo.GetAiringsFromRoomByDate(room, date).ToList();

            foreach (var airingMovie in airingMoviesFromRoomByDate)
            {
                AddMovieDetailsToAiringMovie(airingMovie);
            }

            return airingMoviesFromRoomByDate;
        }

        private void AddMovieDetailsToAiringMovie(AiringMovie airingMovie)
        {
            var movieId = airingMovie.Movie.Id;
            if (movieId == -1) return;

            var movieLogic = new MovieLogic();
            airingMovie.Movie = movieLogic.GetMovieById(movieId);
        }
        private IEnumerable<AiringMovie> OrderAiringsByAiringTime(IEnumerable<AiringMovie> airingMovies)
        {
            return airingMovies.OrderBy(m => m.AiringTime.Date).ToList();
        }

        private DateTime TryToGetAvailableTime(Movie movie, IEnumerable<AiringMovie> airingsInRoom, DateTime date)
        {
            var runTimeToAddAiring = GetRunTimeFromMovie(movie);
            var orderedAirings = OrderAiringsByAiringTime(airingsInRoom).ToList();

            return SearchForAvailableTime(orderedAirings, runTimeToAddAiring, date);
        }

        private DateTime SearchForAvailableTime(List<AiringMovie> orderedAirings, double runTimeToAddAiring, DateTime date)
        {
            const int timeForCleaningInMinutes = 60;
            var lastAiringMovieIndex = orderedAirings.Count() - 1;

            var lastPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            var noTimeAvailable = new DateTime(1990, 12, 12, 12, 12, 12);

            for (var x = 0; x < orderedAirings.Count; x++)
            {
                var runTimeCurrentAiring = GetRunTimeFromMovie(orderedAirings[x].Movie);

                var timeCurrentAiringDone = orderedAirings[x].AiringTime.AddMinutes(runTimeCurrentAiring);
                var timeToAddAiringDone = timeCurrentAiringDone.AddMinutes(timeForCleaningInMinutes + runTimeToAddAiring + timeForCleaningInMinutes);

                if (x != lastAiringMovieIndex)
                {
                    var startTimeNextAiring = orderedAirings[x + 1].AiringTime;

                    if (startTimeNextAiring <= lastPossibleAiringTime && timeToAddAiringDone <= startTimeNextAiring)
                    {
                        return new DateTime(date.Year, date.Month, date.Day, timeCurrentAiringDone.Hour, timeCurrentAiringDone.Minute, timeCurrentAiringDone.Second);
                    }
                }
                else if (timeToAddAiringDone <= lastPossibleAiringTime)
                {
                    return new DateTime(date.Year, date.Month, date.Day, timeCurrentAiringDone.Hour, timeCurrentAiringDone.Minute, timeCurrentAiringDone.Second);
                }
            }

            return noTimeAvailable;
        }

        private void AddAiringMovie(AiringMovie airingMovie, DateTime date)
        {
            const int timeForCleaningInMinutes = 60;
            airingMovie.AiringTime = airingMovie.AiringTime.AddMinutes(timeForCleaningInMinutes);

            _repo.AddAiring(airingMovie);
        }

        private double GetRunTimeFromMovie(Movie movie)
        {
            var runTime = new DateTime(1990, 12, 12);

            var runTimeString = movie.Runtime.Split(' ');

            if (int.TryParse(runTimeString[0], out var totalMinutes))
            {
                runTime = new DateTime(2019, 5, 11, (totalMinutes / 60), (totalMinutes % 60), 0);
            }

            return runTime.TimeOfDay.TotalMinutes;
        }

        /////////////////////TIJDELIJK
        ///
        public double GetRunTimeFromMovie2(Movie movie)
        {
            return GetRunTimeFromMovie(movie);
        }

        public DateTime TryToGetAvailableTime2(Movie movie, List<AiringMovie> sortedAirings, DateTime date)
        {
            return TryToGetAvailableTime(movie, sortedAirings, date);
        }
        public void AddMovieDetailsToAiringMovie2(AiringMovie airingMovie)
        {
            AddMovieDetailsToAiringMovie(airingMovie);
        }

    }
}
