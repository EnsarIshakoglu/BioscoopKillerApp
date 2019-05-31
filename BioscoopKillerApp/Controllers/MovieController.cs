using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using BioscoopKillerApp.Models;
using BioscoopKillerApp.ViewModels;
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
            var model = new AllMoviesViewModel
            {
                Movies = _movieLogic.GetAllMovies(),
                Genres = _movieLogic.GetAllGenres()
            };


            return View(model);
        }

        public IActionResult MovieDetails(Movie movie)
        {
            var movieDetails = new MovieDetailViewModel
            {
                AiringMovies = _movieLogic.GetAiringsFromMovieStartingFromDate(movie, DateTime.Today),
                Movie = _movieLogic.GetMovieById(movie.Id),
                Reviews = _movieLogic.GetAllReviewsFromMovie(movie)
            };

            foreach (var airingMovie in movieDetails.AiringMovies)
            {
                airingMovie.Movie = movie;
            }

            return View(movieDetails);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddMovie()
        {
            return View("AddPage", new Movie());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddAiringMovie()
        {
            var movies = _movieLogic.GetAllMovies();
            var roomTypes = _movieLogic.GetAllRoomTypes();

            var returnModel = new AddAiringMovieViewModel(movies, roomTypes);

            return View("AddPage", returnModel);
        }
        
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

            return View("AddPage", new Movie());
        }
        
        [HttpPost]
        public IActionResult AddAiringMovie([FromBody]AddAiringMovieViewModel addAiringMovieViewModel)
        {
            var movie = _movieLogic.GetAllMovies().First(m => m.Title.Equals(addAiringMovieViewModel.SelectedMovie));
            var returnMessage = "";

            DateTime.TryParse(addAiringMovieViewModel.SelectedDate, out var date);

            if (ModelState.IsValid)
            {
                var addedAiringMovies = 0;
                for (var x = 0; x < Convert.ToInt32(addAiringMovieViewModel.AmountOfTimes); x++)
                {
                    var successful = _movieLogic.TryToAddAiring(movie, date, addAiringMovieViewModel.SelectedRoomType);
                    if (successful) addedAiringMovies++;
                }

                returnMessage = $"Added {addedAiringMovies} airings for {movie.Title}!";
            }

            return new JsonResult(new {message = returnMessage});
        }

        [HttpPost]
        public IActionResult GetMoviesByGenre([FromBody]Movie movie)
        {
            var movies = _movieLogic.GetMoviesByGenre(movie.Genre);

            return PartialView("ShowMovies", movies);
        }

        [HttpPost]
        public IActionResult GetAiringsFromMovieByDate([FromBody] FilterAiringsViewModel model)
        {
            DateTime.TryParse(model.DateString, out var date);

            var airings = _movieLogic.GetAiringsFromMovieByDate(model.Movie, date);

            return PartialView("AiringButtons", airings);
        }
    }
}