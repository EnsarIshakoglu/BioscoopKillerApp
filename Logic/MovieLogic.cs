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
        private readonly MovieRepo _movieRepo = new MovieRepo(new MovieContext());

        private readonly RoomLogic _roomLogic = new RoomLogic();
        private readonly AiringMovieLogic _airingMovieLogic = new AiringMovieLogic();
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();

        public IEnumerable<Movie> GetAllMovies()
        {
            List<Movie> movies = _movieRepo.GetAllMovies().ToList();

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

        public IEnumerable<AiringMovie> GetAiringMoviesFromMovie(Movie movie)
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
    }
}
