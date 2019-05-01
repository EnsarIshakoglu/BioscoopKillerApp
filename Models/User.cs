using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Models
{
    public class User
    {
        public string Email{get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
