using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BioscoopKillerApp.ViewModels
{
    public class AllMoviesViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}
