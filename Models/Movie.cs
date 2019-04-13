using System;
using System.Collections.Generic;

namespace Models
{
    public class Movie
    {
        public Movie()
        {
            Genre = new List<string>();
            Actors = new List<string>();
        }

        public int Id { get; set; }
        public List<string> Genre { get; set; }
        public string Title { get; set; }
        public List<string> Actors { get; set; }
        public string Poster { get; set; }
    }
}
