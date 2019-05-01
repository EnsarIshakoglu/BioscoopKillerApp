using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BioscoopKillerApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic = new TransactionLogic();

        public IActionResult Index(int airingMovieId, int movieId)
        {
            var airingMovie = _transactionLogic.GetAiringMovieById(airingMovieId);
            airingMovie.Movie = _transactionLogic.GetMovieById(movieId);

            return View(airingMovie);
        }
    }
}