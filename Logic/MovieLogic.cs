using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Models;

namespace Logic
{
    public class MovieLogic
    {
        private readonly MovieRepo _movieRepo = new MovieRepo();

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movieRepo.GetAllMovies();
        }

        public IEnumerable<Movie> GetSortedMovies(List<string> categories)
        {
            IEnumerable<Movie> movies = GetAllMovies();

            List<Movie> sortedMovies = new List<Movie>();

            foreach (Movie movie in movies)
            {
                if (movie.Genres.Intersect(categories).Count() == categories.Count())
                    sortedMovies.Add(movie);
            }

            return sortedMovies;
        }
    }
}
