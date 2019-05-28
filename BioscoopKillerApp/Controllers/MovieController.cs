using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic();
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();

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
            var movieDetails = new MovieDetailViewModel
            {
                AiringMovies = _airingMovieLogic.GetAiringMoviesFromMovie(movie),
                Movie = _movieLogic.GetMovieById(movie.Id.GetValueOrDefault()),
                Reviews = _reviewLogic.GetAllReviewsFromMovie(movie)
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
                    var successful = _airingMovieLogic.TryToAddAiring(movie, date, addAiringMovieViewModel.SelectedRoomType);
                    if (successful) addedAiringMovies++;
                }

                returnMessage = $"Added {addedAiringMovies} airings for {movie.Title}!";
            }

            return new JsonResult(new {message = returnMessage});
        }
    }
}