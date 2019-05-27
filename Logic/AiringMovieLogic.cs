﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL;
using Logic.Repositories;
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

        public bool AddAiringMovieSuccessful(Movie movie, DateTime date, string roomType)
        {
            /*
             * Hier hebben we zalen nodig die bv 3d uitzenden -> GetAvailableRoom
             * Kijken of een zaal plek heeft om onze film toe te voegen OF kijken of er uberhaupt een film is
             * Film toevoegen
             */

            var airing = GetRoomWithPlaceForAiring(movie, date, roomType);

            if (airing != null)
            {
                AddAiringMovie(airing, date);
                return true;
            }

            return false;
        }

        private AiringMovie GetRoomWithPlaceForAiring(Movie movie, DateTime date, string roomType)
        {
            var rooms = _roomLogic.GetRoomsByRoomType(roomType);
            var roomDoesNotHavePlace = new DateTime(1990,12,12,12,12,12);

            foreach (var room in rooms)
            {
                var airingsInRoom = GetAiringsFromRoomByDate(room, date);

                if (airingsInRoom.Any())
                {
                    var startTimeAiring = GetAvailableTime(movie, airingsInRoom, date);

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
            var airingMoviesFromRoomByDate = _repo.GetAiringMoviesFromRoomByDate(room, date).ToList();

            foreach (var airingMovie in airingMoviesFromRoomByDate)
            {
                AddMovieToAiringMovie(airingMovie);
            }

            return airingMoviesFromRoomByDate;
        }

        private void AddMovieToAiringMovie(AiringMovie airingMovie)
        {
            var movieId = airingMovie.Movie.Id.GetValueOrDefault(-1);
            if (movieId == -1) return;
            airingMovie.Movie = _movieLogic.GetMovieById(movieId);
        }
        private IEnumerable<AiringMovie> OrderAiringsByAiringTime(IEnumerable<AiringMovie> airingMovies)
        {
            return airingMovies.OrderBy(m => m.AiringTime.GetValueOrDefault().Date).ToList();
        }

        private DateTime GetAvailableTime(Movie movie, IEnumerable<AiringMovie> airingsInRoom, DateTime date)
        {
            var sortedAirings = OrderAiringsByAiringTime(airingsInRoom).ToList();
            var lastAiringMovieIndex = sortedAirings.Count - 1;
            const int timeForCleaningInMinutes = 60;

            var lastPossibleAiringTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            var runTimeToAddAiring = GetRunTimeFromMovie(movie);

            for (var x = 0; x < sortedAirings.Count; x++)
            {
                var runTimeCurrentAiring = GetRunTimeFromMovie(sortedAirings[x].Movie);

                var timeCurrentAiringDone = sortedAirings[x].AiringTime.GetValueOrDefault().AddMinutes(runTimeCurrentAiring);
                var timeToAddAiringDone = timeCurrentAiringDone.AddMinutes(timeForCleaningInMinutes + runTimeToAddAiring + timeForCleaningInMinutes);

                if (x != lastAiringMovieIndex)
                {
                    var startTimeNextAiring = sortedAirings[x + 1].AiringTime;

                    if (startTimeNextAiring <= lastPossibleAiringTime && timeToAddAiringDone <= startTimeNextAiring)
                    {
                        return new DateTime(date.Year, date.Month, date.Day, timeCurrentAiringDone.Hour, timeCurrentAiringDone.Minute, timeCurrentAiringDone.Second);
                    }
                }
                else
                {
                    if (timeToAddAiringDone <= lastPossibleAiringTime)
                    {
                        return new DateTime(date.Year, date.Month, date.Day, timeCurrentAiringDone.Hour, timeCurrentAiringDone.Minute, timeCurrentAiringDone.Second);
                    }
                }
            }

            return new DateTime(1990, 12, 12, 12, 12, 12);
        }

        private void AddAiringMovie(AiringMovie airingMovie, DateTime date)
        {
            const int timeForCleaningInMinutes = 60;
            airingMovie.AiringTime = airingMovie.AiringTime.GetValueOrDefault().AddMinutes(timeForCleaningInMinutes);

            _repo.AddAiringMovie(airingMovie);
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
