using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;

namespace BioscoopKillerApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic = new TransactionLogic();

        [Route("/Transaction/Index/{airingMovieId}/{movieId}")]
        public IActionResult Index(int airingMovieId, int movieId)
        {
            var airingMovie = _transactionLogic.GetAiringMovieById(airingMovieId);
            airingMovie.Movie = _transactionLogic.GetMovieById(movieId);

            return View(airingMovie);
        }

        [HttpPost]
        public IActionResult SaveReservation([FromBody]Reservation reservation)
        {
            _transactionLogic.SaveReservation(reservation);

            return new JsonResult(new { message = $"Created a reservation for {reservation.SeatNumbers.Length} seat(s) with the e-mail address {reservation.MailAddress}!"});
        }

        public bool CheckIfValidEmail(string email)
        {
            Regex rx = new Regex(
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return rx.IsMatch(email);
        }
    }
}