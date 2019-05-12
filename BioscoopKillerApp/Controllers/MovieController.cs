using System;
using System.Linq;
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
        [HttpGet]
        public IActionResult AddMovie()
        {
            return View("AddPage", new Movie());
        }

        [HttpGet]
        public IActionResult AddAiringMovieView()
        {
            var movies = _movieLogic.GetAllMovies();
            var roomTypes = _movieLogic.GetAllRoomTypes();

            var returnModel = new AddAiringMovieViewModel(movies, roomTypes);

            return View("AddPage", returnModel);
        }

        //[Authorize(Roles = "Admin")]
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

            return RedirectToAction("AddMovie");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAiringMovie([FromBody]AddAiringMovieViewModel addAiringMovieViewModel)
        {
            var a = "";

            var airingMovie = new AiringMovie
            {
                Movie =
                    _movieLogic.GetAllMovies().First(m => m.Title.Equals(addAiringMovieViewModel.SelectedMovie)),
                Room = new Room{Type = addAiringMovieViewModel.SelectedRoomType}
            };

            DateTime.TryParse(addAiringMovieViewModel.SelectedDate, out var date);

            if (ModelState.IsValid)
            {
                for (var x = 0; x < Convert.ToInt32(addAiringMovieViewModel.AmountOfTimes); x++)
                {
                    a = _movieLogic.AddAiringMovie(airingMovie, date);
                }
            }

            TempData["alertMessage"] = a; //add airing movie

            return View("AddAiringMovie");
        }
    }
}