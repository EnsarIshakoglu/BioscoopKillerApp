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

            var airingMovies = GetAiringMoviesByRoomType(airingMovie.Room.Type).Where(m => m.AiringTime.Date.DayOfWeek.Equals(date.DayOfWeek));

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

        private string GetAvailableTime(AiringMovie airingMovie, IEnumerable<AiringMovie> airingMovies, DateTime date)
        {
            var sortedMovies = airingMovies.OrderBy(m => m.AiringTime.Date).ToList();
            var lastPossibleAiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59)
                .TimeOfDay;
            var lastMovieIndex = sortedMovies.Count - 1;

            for (var x = 0; x < sortedMovies.Count; x++)
            {
                var runTimeCurrentMovie = GetRunTimeFromMovie(sortedMovies[x].Movie);
                var runTimeToAddMovie = GetRunTimeFromMovie(airingMovie.Movie);

                var timeCurrentAiringDone = sortedMovies[x].AiringTime.AddMinutes(runTimeCurrentMovie);
                var timeAiringDone = timeCurrentAiringDone.AddMinutes(60 + runTimeToAddMovie + 60).TimeOfDay;

                if (x != lastMovieIndex)
                {
                    var startTimeNextMovie = sortedMovies[x + 1].AiringTime.TimeOfDay;

                    if (startTimeNextMovie <= lastPossibleAiringTime)
                    {
                        if (timeAiringDone > startTimeNextMovie) continue;
                        //_repo.AddAiringMovie(airingMovie, timeCurrentAiringDone.AddMinutes(60));
                        return $"AiringMovie created for {timeCurrentAiringDone.AddMinutes(60).ToString("f", CultureInfo.GetCultureInfo("en-US"))}";
                    }
                    else
                    {
                        return
                            $"There are no free places left for {date.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}";
                    }
                }
                else
                {
                    if (timeCurrentAiringDone.TimeOfDay <= lastPossibleAiringTime)
                    {
                        //_repo.AddAiringMovie(airingMovie, timeCurrentAiringDone.AddMinutes(60));
                        return $"AiringMovie created for {timeCurrentAiringDone.AddMinutes(60).ToString("f", CultureInfo.GetCultureInfo("en-US"))}";
                    }
                    else
                    {
                        return
                            $"There are no free places left for {date.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}";
                    }
                }
            }

            return
                $"There are no free places left for {date.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}";
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
