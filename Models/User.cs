using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;

namespace Models
{
    public class User
    {
        [Required(ErrorMessage = "Email field is required!")]
        public string Email{get; set; }
        [Required(ErrorMessage = "Password field is required!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Name field is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname field is required!")]
        public string SurName { get; set; }
    }
}
