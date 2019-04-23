using System;
using System.Collections.Generic;

namespace Models
{
    public class Movie
    {
        public Movie()
        {
            Genres = new List<string>();
            Cast = new List<string>();
        }

        public int Id { get; set; }
        public string Runtime { get; set; }
        public List<string> Genres { get; set; }
        public int PublishedYear { get; set; }
        public string Title { get; set; }
        public List<string> Cast { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public DateTime Duration { get; set; }
    }
}
