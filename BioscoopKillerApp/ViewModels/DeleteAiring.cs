using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BioscoopKillerApp.ViewModels
{
    public class DeleteAiring
    {
        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
        public IEnumerable<string> Genres { get; set; } = new List<string>(); //kijken of dit belangrijk is (miss kunnen we gwn met ajax ophalen, 1 viewmodel minder :))
    }
}
