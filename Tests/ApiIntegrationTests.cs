using System;
using System.Collections.Generic;
using System.Text;
using API;
using Logic;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class ApiIntegrationTests
    {
        private ApiHelper _apiHelper;

        private readonly Movie _existingMovie = new Movie
        {
            Title = "Aquaman",
            PublishedYear = 2018
        };
        private readonly Movie _nonExistingMovie = new Movie
        {
            Title = "sefsdf234234esedfsd",
            PublishedYear = 2018
        };

        [SetUp]
        public void Setup()
        {
            _apiHelper = new ApiHelper();
        }

        [Test]
        public void AddApiDataToMovieSuccessfulExistingMovie()
        {
            _apiHelper.AddApiDataToMovie(_existingMovie).Wait();
            var expectedGenres = "Action, Adventure, Fantasy, Sci-Fi";

            Assert.AreEqual(expectedGenres, _existingMovie.Genre);
        }

        [Test]
        public void AddApiDataToMovieUnSuccessfulNonExistingMovie()
        {
            _apiHelper.AddApiDataToMovie(_nonExistingMovie).Wait();

            Assert.AreEqual(null, _nonExistingMovie.Genre);
        }
    }
}
