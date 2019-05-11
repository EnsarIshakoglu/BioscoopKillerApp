﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using BioscoopKillerApp.Models;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Models;
using Models.Enums;

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
        [Authorize(Roles= "Admin")]
        public IActionResult AddPage()
        {
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
        public IActionResult AddAiringMovie([Bind("Movie, RoomType")] AiringMovie airingMovie, int amountOfTimes)
        {
            if (ModelState.IsValid)
            {
                for (var x = 0; x < amountOfTimes; x++)
                {
                    _movieLogic.AddAiringMovie(airingMovie);
                }
            }

            return View("AddPage");
        }
    }
}