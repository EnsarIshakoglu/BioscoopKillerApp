using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL;
using Models;
using Models.Enums;

namespace Logic
{
    public class AiringMovieLogic
    {
        private readonly AiringMovieRepo _repo = new AiringMovieRepo();

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            return _repo.GetAiringMoviesFromMovie(movie);
        }

        public IEnumerable<AiringMovie> GetAiringMoviesByRoomType(string roomType)
        {
            return _repo.GetAiringMoviesByRoomType(roomType);
        }

        public AiringMovie GetAiringMovieById(int id)
        {
            return _repo.GetAiringMovieById(id);
        }

        public string AddAiringMovie(AiringMovie airingMovie, DateTime date, IEnumerable<Movie> allMovies)
        {
            var toReturnString = "";

            var airingMovies = GetAiringMoviesByRoomType(airingMovie.Room.Type).Where(m => m.AiringTime.Date.DayOfWeek.Equals(date.DayOfWeek)).ToList();

            if (airingMovies.Count == 0)
            {
                return FirstAiringOfTheDay(airingMovie, date);
            }

            foreach (var airing in airingMovies)
            {
                airing.Movie = allMovies.First(m => m.Title.Equals(airing.Movie.Title));
            }

            var airingRooms = airingMovies.GroupBy(movie => movie.Room.Number);

            foreach (var airingRoom in airingRooms)
            {
                toReturnString = GetAvailableTime(airingMovie, airingRoom, date);
            }

            return toReturnString;
        }

        private string FirstAiringOfTheDay(AiringMovie airingMovie, DateTime date)
        {
            var earliestPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 11, 0, 0);

            _repo.AddAiringMovie(airingMovie, earliestPossibleAiringTime);

            return $"AiringMovie created for {earliestPossibleAiringTime.ToString("f", CultureInfo.GetCultureInfo("en-US"))}";
        }

        private string GetAvailableTime(AiringMovie airingMovie, IEnumerable<AiringMovie> airingMovies, DateTime date)
        {
            var sortedMovies = airingMovies.OrderBy(m => m.AiringTime.Date).ToList();
            var lastAiringMovieIndex = sortedMovies.Count - 1;
            const int timeForCleaningInMinutes = 60;

            var lastPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
                .TimeOfDay;
            var runTimeToAddAiring = GetRunTimeFromMovie(airingMovie.Movie);

            for (var x = 0; x < sortedMovies.Count; x++)
            {
                var runTimeCurrentAiring = GetRunTimeFromMovie(sortedMovies[x].Movie);

                var timeCurrentAiringDone = sortedMovies[x].AiringTime.AddMinutes(runTimeCurrentAiring);
                var timeToAddAiringDone = timeCurrentAiringDone.AddMinutes(timeForCleaningInMinutes + runTimeToAddAiring + timeForCleaningInMinutes).TimeOfDay;

                if (x != lastAiringMovieIndex)
                {
                    var startTimeNextAiring = sortedMovies[x + 1].AiringTime.TimeOfDay;

                    if (startTimeNextAiring <= lastPossibleAiringTime && timeToAddAiringDone <= startTimeNextAiring)
                    {
                        return AiringMovieCreationSuccessful(airingMovie, date, timeCurrentAiringDone);
                    }
                }
                else
                {
                    if (timeCurrentAiringDone.TimeOfDay <= lastPossibleAiringTime)
                    {
                        return AiringMovieCreationSuccessful(airingMovie, date, timeCurrentAiringDone);
                    }
                }
            }

            return AiringMovieCreationUnSuccessful(date);
        }

        private string AiringMovieCreationSuccessful(AiringMovie airingMovie, DateTime date, DateTime airingDonePreviousMovie)
        {
            const int timeForCleaningInMinutes = 60;
            var startTimeAiring = new DateTime(date.Year, date.Month, date.Day, airingDonePreviousMovie.Hour, airingDonePreviousMovie.Minute, airingDonePreviousMovie.Second);
            startTimeAiring = startTimeAiring.AddMinutes(timeForCleaningInMinutes);

            _repo.AddAiringMovie(airingMovie, startTimeAiring);

            return $"AiringMovie created for {startTimeAiring.ToString("f", CultureInfo.GetCultureInfo("en-US"))}";
        }

        private string AiringMovieCreationUnSuccessful(DateTime date)
        {
            return $"There are no free places left for {date.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}";
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
