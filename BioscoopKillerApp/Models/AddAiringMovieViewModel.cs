using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace BioscoopKillerApp.Models
{
    public class AddAiringMovieViewModel
    {
        public AddAiringMovieViewModel(IEnumerable<Movie> movies, IEnumerable<string> roomTypes)
        {
            var temporaryMoviesList = movies.Select(movie => new SelectListItem {Text = movie.Title}).ToList();
            var temporaryRoomTypeList = roomTypes.Select(roomType => new SelectListItem { Text = roomType }).ToList();

            Movies = temporaryMoviesList;
            RoomTypes = temporaryRoomTypeList;
        }

        public List<SelectListItem> Movies { get; set; }
        public IEnumerable<SelectListItem> RoomTypes { get; set; }

        public string SelectedMovie { get; set; }
        public string SelectedRoomType { get; set; }
        public string SelectedDate { get; set; }
        public string AmountOfTimes { get; set; }
    }
}
