using System;
using System.Collections.Generic;
using DAL;
using DAL.MockContexts;
using Logic;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private AiringMovieLogic _logic;

        [SetUp]
        public void Setup()
        {
            _logic= new AiringMovieLogic(new MockAiringMovieContext(), new MockRoomContext());
        }

        [Test]
        public void GetRunTimeFromMovieTestCorrect()
        {
            var movie = new Movie
            {
                Runtime = "130 min"
            };

            var runtime = _logic.GetRunTimeFromMovie2(movie);
            var expectedOutcome = 130;

            Assert.AreEqual(expectedOutcome, runtime);
        }

        [Test]
        public void GetRunTimeFromMovieTestWrongOnlyText()
        {
            var movie = new Movie
            {
                Runtime = "test"
            };

            var runtime = _logic.GetRunTimeFromMovie2(movie);
            var expectedOutcome = 0;

            Assert.AreEqual(expectedOutcome, runtime);
        }

        [Test]
        public void GetRunTimeFromMovieTestWrongNoInput()
        {
            var movie = new Movie
            {
                Runtime = ""
            };

            var runtime = _logic.GetRunTimeFromMovie2(movie);
            var expectedOutcome = 0;

            Assert.AreEqual(expectedOutcome, runtime);
        }

        [Test]
        public void TryToGetAvailableTimeTestCorrect()
        {
            var airingsInRoom = new List<AiringMovie>
            {
                new AiringMovie
                {
                    Movie = new Movie
                    {
                        Runtime = "120 min"
                    },
                    AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0)
                },
                new AiringMovie
                {
                    Movie = new Movie
                    {
                        Runtime = "130 min"
                    },
                    AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15, 0, 0)
                }
            };

            var movie = new Movie
            {
                Runtime = "150 min"
            };

            var outputAlgorithm = _logic.TryToGetAvailableTime2(movie, airingsInRoom, DateTime.Today);
            var expectedOutcome = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 10, 0);

            Assert.AreEqual(expectedOutcome, outputAlgorithm);
        }

        [Test]
        public void TryToGetAvailableTimeTestWithNoAvailableTime()
        {
            var airingsInRoom = new List<AiringMovie>
            {
                new AiringMovie
                {
                    Movie = new Movie
                    {
                        Runtime = "130 min"
                    },
                    AiringTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 21, 20, 0)
                }
            };

            var movie = new Movie
            {
                Runtime = "150 min"
            };

            var outputAlgorithm = _logic.TryToGetAvailableTime2(movie, airingsInRoom, DateTime.Today);
            var noTimeAvailable = new DateTime(1990, 12, 12, 12, 12, 12);

            Assert.AreEqual(noTimeAvailable, outputAlgorithm);
        }

        [Test]
        public void TryToAddAiringTestSuccessfulSameRoom()
        {
            var toAddMovie = new Movie
            {
                Id = 101,
                Title = "Pokémon: Detective Pikachu",
                MoviePrice = 10,
                Runtime = "120 min"
            };
            var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var roomType = "IMAX 3D";

            var outcome = _logic.TryToAddAiring(toAddMovie, date, roomType);

            Assert.AreEqual(true, outcome);
        }

        [Test]
        public void TryToAddAiringTestSuccessfulFindNewRoom()
        {
            var toAddMovie = new Movie
            {
                Id = 101,
                Title = "Pokémon: Detective Pikachu",
                MoviePrice = 10,
                Runtime = "120 min"
            };
            var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var roomType = "IMAX 2D";

            var outcome = _logic.TryToAddAiring(toAddMovie, date, roomType);

            Assert.AreEqual(true, outcome);
        }
        [Test]
        public void TryToAddAiringTestUnSuccessfulNoPlaceForAiring()
        {
            var toAddMovie = new Movie
            {
                Id = 101,
                Title = "Pokémon: Detective Pikachu",
                MoviePrice = 10,
                Runtime = "120 min"
            };
            var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var roomType = "2D";

            var outcome = _logic.TryToAddAiring(toAddMovie, date, roomType);

            Assert.AreEqual(true, outcome);
        }
    }
}