using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class MovieLogic
    {
        private readonly MovieRepo _repo = new MovieRepo(new MovieContext());

        private readonly RoomLogic _roomLogic = new RoomLogic();
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic();
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();

        public IEnumerable<Movie> GetAllMovies()
        {
            List<Movie> movies = _repo.GetAllMovies().ToList();

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

        public IEnumerable<string> GetAllRoomTypes()
        {
            return _roomLogic.GetAllRoomTypes();
        }

        public Movie GetMovieById(int movieId)
        {
            return _repo.GetMovieById(movieId);
        }

        public void AddMovie(Movie movie)
        {
            _repo.AddMovie(movie);
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            return _repo.CheckIfMovieExists(movie);
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovie(Movie movie)
        {
            return _airingMovieLogic.GetAiringMoviesFromMovie(movie);
        }

        public IEnumerable<Review> GetAllReviewsFromMovie(Movie movie)
        {
            return _reviewLogic.GetAllReviewsFromMovie(movie);
        }

        public bool TryToAddAiring(Movie movie, DateTime date, string selectedRoomType)
        {
            return _airingMovieLogic.TryToAddAiring(movie, date, selectedRoomType);
        }

        public IEnumerable<AiringMovie> GetAiringsFromMovieByDate(Movie movie, DateTime date)
        {
            return _airingMovieLogic.GetAiringsFromMovieByDate(movie, date);
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            return _repo.GetMoviesBySearchParam(searchParam);
        }

        public AiringMovie GetAiringById(AiringMovie airing)
        {
            var toReturnAiring = _airingMovieLogic.GetAiringMovieById(airing.Id);
            toReturnAiring.Movie = GetMovieById(airing.Movie.Id);

            return toReturnAiring;
        }
        /// <summary>
        /// Returns airings for given movie starting from given date.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<AiringMovie> GetAiringsFromMovieStartingFromDate(Movie movie, DateTime date)
        {
            return _airingMovieLogic.GetAiringsFromMovieStartingFromDate(movie, date);
        }

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            return _repo.GetMoviesByGenre(category);
        }

        public IEnumerable<string> GetAllGenres()
        {
            return _repo.GetAllGenres();
        }
    }
}
