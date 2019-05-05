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
        public string MailAddress { get; set; }
        public string[] SeatNumbers { get; set; }
        public int AiringMovieId { get; set; }
    }
}
