﻿using System;
using System.Collections.Generic;
using Interfaces;
using Models;

namespace Logic.Repositories
{
    public class MovieRepo
    {
        private readonly IMovieContext _movieContext;

        public MovieRepo(IMovieContext context)
        {
            _movieContext = context;
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movieContext.GetAllMovies();
        }

        public void DeleteMovie(Movie movie)
        {
            _movieContext.DeleteMovie(movie);
        }

        public Movie GetMovieById(int movieId)
        {
            return _movieContext.GetMovieById(movieId);
        }

        public void AddMovie(Movie movie)
        {
            _movieContext.AddMovie(movie);
        }

        public bool CheckIfMovieExists(Movie movie)
        {
            return _movieContext.CheckIfMovieExists(movie);
        }

        public IEnumerable<Movie> GetMoviesByGenre(string category)
        {
            return _movieContext.GetMoviesByGenre(category);
        }

        public IEnumerable<string> GetAllGenres()
        {
            return _movieContext.GetAllGenres();
        }

        public IEnumerable<Movie> GetMoviesBySearchParam(string searchParam)
        {
            return _movieContext.GetMoviesBySearchParam(searchParam);
        }
    }
}
