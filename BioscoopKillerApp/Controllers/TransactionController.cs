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

        [HttpPost]
        public IActionResult GoToReservationPage([FromBody] AiringMovie airing)
        {
            return new JsonResult(new { response = "Redirect", url = Url.Action("Index", "Transaction", airing)});
        }

        [HttpPost]
        public IActionResult SaveReservation([FromBody]Reservation reservation)
        {
            _transactionLogic.SaveReservation(reservation);

            return new JsonResult(new { message = $"Created a reservation for {reservation.Seats.Length} seat(s) with the e-mail address {reservation.MailAddress}!"});
        }

        public IActionResult Index(AiringMovie model)
        {
            var airing = _transactionLogic.GetAiringMovieById(model.Id);

            return View("Index", airing);
        }

        public bool CheckIfValidEmail(string email)
        {
            Regex rx = new Regex(
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return rx.IsMatch(email);
        }
    }
}