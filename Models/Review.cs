using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Review
    {
        [Required]
        [MinLength(6, ErrorMessage = "Review text needs to be at least 6 characters long!")]
        [MaxLength(2000, ErrorMessage = "Review text can only be 2000 characters long!")]
        public string ReviewText { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Review title needs to be at least 6 characters long!")]
        [MaxLength(100, ErrorMessage = "Review title can only be 100 characters long!")]
        public string ReviewTitle { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
