using System;
using System.Collections.Generic;

namespace Models
{
    public class Movie
    {
        public int MoviePrice { get; set; }
        public int Id { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public int PublishedYear { get; set; }
        public string Title { get; set; }
        public string Actors { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
    }
}
