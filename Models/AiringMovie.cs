using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class AiringMovie
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public Room Room { get; set; }
        public DateTime AiringTime { get; set; }
        public int Price { get; set; }
    }
}
