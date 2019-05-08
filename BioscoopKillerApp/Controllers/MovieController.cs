using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult MovieDetails(Movie movie)
        {
            MovieDetailViewModel movieDetails = new MovieDetailViewModel
            {
                AiringMovies = _movieLogic.GetAiringMovies(movie),
                Movie = movie
            };

            foreach (var airingMovie in movieDetails.AiringMovies)
            {
                airingMovie.Movie = movie;
            }

            return View(movieDetails);
        }

        public IActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddMovie([Bind("Title, PublishedYear, MoviePrice")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieLogic.AddMovie(movie);
            }

            return RedirectToAction("AddPage");
        }
    }
}