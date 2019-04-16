using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
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
            return View(movie);
        }
    }
}