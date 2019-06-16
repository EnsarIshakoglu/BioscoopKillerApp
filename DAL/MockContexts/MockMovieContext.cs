using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace DAL.MockContexts
{
    public class MockMovieContext : IMovieContext
    {
        private IEnumerable<Movie> _movies = new List<Movie>
        {
            new Movie
            {
                Id = 1,
                Title = "Avengers",
                Runtime = "130 min",
                Genre = "Horror, Action"
            },
            new Movie
            {
                Id = 2,
                Title = "Finding nemo",
                Runtime = "130 min",
                Genre = "Comedy, Action"
            },
            new Movie
            {
                Id = 3,
                Title = "X-men",
                Runtime = "120 min",
                Genre = "Comedy, Adventure, Action"
            },
            new Movie
            {
                Id = 4,
                Title = "John Wick",
                Runtime = "100 min",
                Genre = "Musical, Comedy, Adventure"
            },
            new Movie
            {
                Id = 4,
                Title = "Godzilla",
                Runtime = "100 min",
                Genre = "Musical, Comedy, Adventure"
            }
        };

        public IEnumerable<Movie> GetAllMovies()
        {
            throw new NotImplementedException();
        }

        public Movie GetMovieById(int movieId)
        {
            return _movies.First(m => m.Id.Equals(movieId));
        }

        public void AddMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllGenres()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            throw new NotImplementedException();
        }

        public void DeleteMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
