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
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic();

        public IEnumerable<Movie> GetAllMovies()
        {
            List<Movie> movies = _movieRepo.GetAllMovies().ToList();

            foreach (var movie in movies)
            {
                int totalMinutes = Convert.ToInt32(movie.Runtime.Substring(0, movie.Runtime.IndexOf(" ", StringComparison.Ordinal)));
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                movie.Runtime = $"{hours} h {minutes} m";
            }

            return movies;
        }

        public IEnumerable<Movie> GetSortedMovies(List<string> categories)
        {
            IEnumerable<Movie> movies = GetAllMovies();

            List<Movie> sortedMovies = new List<Movie>();

            /*foreach (Movie movie in movies)
            {
                if (movie.Genre.Intersect(categories).Count() == categories.Count())
                    sortedMovies.Add(movie);
            }*/

            return sortedMovies;
        }

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
        {
            return _airingMovieLogic.GetAiringMoviesFromMovie(movie);
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieRepo.GetMovieById(movieId);
        }

        public void AddMovie(Movie movie)
        {
            _movieRepo.AddMovie(movie);
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            return _movieRepo.CheckIfMovieExists(movie);
        }

        public void AddAiringMovie(AiringMovie airingMovie)
        {
            _airingMovieLogic.AddAiringMovie(airingMovie);
        }
    }
}
