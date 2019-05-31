using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Movie
    {
        [Range(5, 15, ErrorMessage = "Range should be between 5 en 15 euro's!")]
        public decimal MoviePrice { get; set; }
        public int Id { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        [Range(1950, 2025, ErrorMessage = "Range should be between 1950 en 2025!")]
        public int? PublishedYear { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        public string Actors { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
    }
}
