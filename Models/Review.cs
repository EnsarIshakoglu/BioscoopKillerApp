using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Review
    {
        public string ReviewText { get; set; }
        public Movie Movie { get; set; }
        public int UserId { get; set; }

    }
}
