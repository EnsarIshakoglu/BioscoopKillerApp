using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL;
using DAL.Contexts;
using Models;
using Models.Enums;

namespace Logic
{
    public class AiringMovieLogic
    {
        private readonly AiringMovieRepo _repo = new AiringMovieRepo(new AiringMovieContext());
        private readonly RoomLogic _roomLogic = new RoomLogic();
        private readonly MovieLogic _movieLogic = new MovieLogic();

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            return _repo.GetAiringMoviesFromMovie(movie);
        }
        public AiringMovie GetAiringMovieById(int id)
        {
            return _repo.GetAiringMovieById(id);
        }

        public bool AddAiringMovie(AiringMovie chosenAiring, DateTime date)
        {
            var allMovies = _movieLogic.GetAllMovies();
            var airingMovie = chosenAiring;//----------------------------------------------FF TESTEN
            var airingMovies = _roomLogic.GetAiringMoviesByRoomType(airingMovie.Room.Type).Where(m => m.AiringTime.HasValue && m.AiringTime.Value.Date.DayOfWeek.Equals(date.DayOfWeek)).ToList();/*
            airingMovies.AddRange(_roomLogic.GetAiringMoviesByRoomType(airingMovie.Room.Type).Where(m => !m.AiringTime.HasValue).ToList());*/

            var roomIds = _roomLogic.GetRoomIdsByRoomType(airingMovie.Room.Type);
            var airingRooms = new Dictionary<Room, IEnumerable<AiringMovie>>();

            foreach (var roomId in roomIds)
            {
                airingRooms.Add(new Room
                {
                    Number = roomId
                }, airingMovies.Where(m => m.Room.Number.Equals(roomId)));
            }

            foreach (var airingRoom in airingRooms.Where(m => m.Value.Any()))
            {
                foreach (var airing in airingRoom.Value)
                {
                    airing.Movie = allMovies.First(m => m.Title.Equals(airing.Movie.Title));
                }
            }

            foreach (var airingRoom in airingRooms)
            {
                if (!airingRoom.Value.Any())
                {
                    return AddAiringAsFirstAiringOfTheDay(airingMovie, date, airingRoom.Key);
                }

                if (GetAvailableTime(airingMovie, airingRoom.Value, date))
                {
                    return true;
                }
            }

            /*if (airingMovies.Count == 0)
            {
                return AddAiringAsFirstAiringOfTheDay(airingMovie, date);
            }

            foreach (var airing in airingMovies.Where(m => m.AiringTime.HasValue))
            {
                airing.Movie = allMovies.First(m => m.Title.Equals(airing.Movie.Title));
            }

            //var airingRooms = airingMovies.GroupBy(movie => movie.Room.Number).ToList();

            foreach (var room in airingRooms)
            {
                if (room.Any(airing => !airing.AiringTime.HasValue))
                {
                    //dictionary maken met <Room, AiringMovie>
                    var index = airingRooms.FindIndex(room);
                    return AddAiringAsFirstAiringOfTheDay(airingMovie, room.ElementAt(), date);
                }

                if (GetAvailableTime(airingMovie, room, date))
                {
                    return true;
                }
            }*/

            return false;
        }

        private bool AddAiringAsFirstAiringOfTheDay(AiringMovie airingMovie,  DateTime date, Room room)
        {
            airingMovie.Room = room;
            var earliestPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 11, 0, 0);

            _repo.AddAiringMovie(airingMovie, earliestPossibleAiringTime);

            return true;
        }

        private bool GetAvailableTime(AiringMovie airingMovie, IEnumerable<AiringMovie> airingMovies, DateTime date)
        {
            var sortedMovies = airingMovies.OrderBy(m => m.AiringTime.Value.Date).ToList();
            var lastAiringMovieIndex = sortedMovies.Count - 1;
            const int timeForCleaningInMinutes = 60;

            var lastPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            var runTimeToAddAiring = GetRunTimeFromMovie(airingMovie.Movie);

            for (var x = 0; x < sortedMovies.Count; x++)
            {
                var runTimeCurrentAiring = GetRunTimeFromMovie(sortedMovies[x].Movie);

                var timeCurrentAiringDone = sortedMovies[x].AiringTime.Value.AddMinutes(runTimeCurrentAiring);
                var timeToAddAiringDone = timeCurrentAiringDone.AddMinutes(timeForCleaningInMinutes + runTimeToAddAiring + timeForCleaningInMinutes);

                if (x != lastAiringMovieIndex)
                {
                    var startTimeNextAiring = sortedMovies[x + 1].AiringTime;

                    if (startTimeNextAiring <= lastPossibleAiringTime && timeToAddAiringDone <= startTimeNextAiring)
                    {
                        return AiringMovieCreationSuccessful(airingMovie, date, timeCurrentAiringDone);
                    }
                }
                else
                {
                    if (timeToAddAiringDone <= lastPossibleAiringTime)
                    {
                        return AiringMovieCreationSuccessful(airingMovie, date, timeCurrentAiringDone);
                    }
                }
            }

            return AiringMovieCreationUnSuccessful();
        }

        private bool AiringMovieCreationSuccessful(AiringMovie airingMovie, DateTime date, DateTime airingDonePreviousMovie)
        {
            const int timeForCleaningInMinutes = 60;
            var startTimeAiring = new DateTime(date.Year, date.Month, date.Day, airingDonePreviousMovie.Hour, airingDonePreviousMovie.Minute, airingDonePreviousMovie.Second);
            startTimeAiring = startTimeAiring.AddMinutes(timeForCleaningInMinutes);

            _repo.AddAiringMovie(airingMovie, startTimeAiring);

            //return $"AiringMovie created for {startTimeAiring.ToString("f", CultureInfo.GetCultureInfo("en-US"))}";
            return true;
        }

        private bool AiringMovieCreationUnSuccessful()
        {
            //return $"There are no free places left for {date.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}";
            return false;
        }

        private static double GetRunTimeFromMovie(Movie movie)
        {
            var runTime = new DateTime(2019, 5, 11);

            var runTimeString = movie.Runtime.Split(' ');

            if (int.TryParse(runTimeString[0], out var totalMinutes))
            {
                runTime = new DateTime(2019, 5, 11, (totalMinutes / 60), (totalMinutes % 60), 0);
            }

            return runTime.TimeOfDay.TotalMinutes;
        }
    }
}
