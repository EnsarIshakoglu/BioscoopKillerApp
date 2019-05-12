using System;
using BioscoopKillerApp.Models;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BioscoopKillerApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieLogic _movieLogic = new MovieLogic();

        public IActionResult Index()
        {
            return View(_movieLogic.GetAllMovies());
        }
        public IActionResult AllMovies()
        {
            return View(_movieLogic.GetAllMovies());

        }

        public IActionResult MovieDetails(Movie movie)
        {
            MovieDetailViewModel movieDetails = new MovieDetailViewModel
            {
                AiringMovies = _movieLogic.GetAiringMoviesFromMovie(movie),
                Movie = movie
            };

            foreach (var airingMovie in movieDetails.AiringMovies)
            {
                airingMovie.Movie = movie;
            }

            return View(movieDetails);

        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddPage()
        {
            var date = new DateTime(2018, 4, 28, 21, 30, 0);
            AddAiringMovie(_movieLogic.GetAiringMovieById(1), 1, date);

            return View();

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddMovie([Bind("Title, PublishedYear, MoviePrice")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                if (_movieLogic.CheckIfMovieExists(movie))
                {
                    _movieLogic.AddMovie(movie);
                }
                else
                {
                    TempData["alertMessage"] = "Movie does not exist!";
                }

            }

            return View("AddPage");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddAiringMovie([Bind("Movie, RoomType")] AiringMovie airingMovie, int amountOfTimes, DateTime date)
        {
            string a = "";
            airingMovie.Movie = _movieLogic.GetMovieById(5);

            if (ModelState.IsValid)
            {
                for (var x = 0; x < amountOfTimes; x++)
                {
                    a = _movieLogic.AddAiringMovie(airingMovie, date);
                }
            }

            TempData["alertMessage"] = a; //add airing movie

            return View("AddPage");
        }
    }
}