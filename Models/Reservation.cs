using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;

namespace Models
{
    public class Reservation
    {
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        public string MailAddress { get; set; }
        public Seat[] Seats { get; set; }
        public AiringMovie AiringMovie { get; set; }
    }
}
