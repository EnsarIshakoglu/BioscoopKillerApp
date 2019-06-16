using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BioscoopKillerApp.Models
{
    public class MovieDetailViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<AiringMovie> AiringMovies { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
