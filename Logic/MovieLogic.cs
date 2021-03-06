﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.MockContexts;
using Interfaces;
using Interfaces.ContextInterfaces;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class MovieLogic
    {
        public MovieLogic(IMovieContext context, IRoomContext roomContext, IApiHelper apiHelper, IAiringMovieContext airingMovieContext)
        {
            _repo = new MovieRepo(context, apiHelper);
            _roomLogic = new RoomLogic(roomContext);
            _airingMovieLogic = new AiringMovieLogic(airingMovieContext, roomContext);
            _reviewLogic = new ReviewLogic();
        }

        private readonly MovieRepo _repo;

        private readonly RoomLogic _roomLogic;
        private readonly AiringMovieLogic _airingMovieLogic;
        private readonly ReviewLogic _reviewLogic;

        public IEnumerable<Movie> GetAllMovies()
        {
            return _repo.GetAllMovies().ToList();
        }

        public void DeleteMovie(Movie movie)
        {
            _repo.DeleteMovie(movie);
        }

        public void DeleteAiring(AiringMovie airing)
        {
            _airingMovieLogic.DeleteAiring(airing);
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

        public bool CheckIfMovieExists(Movie movie)
        {
            var movieExists = false;

            _repo.AddApiData(movie);

            if (movie.Poster != null)
            {
                movieExists = true;
            }

            return movieExists;
        }
    }
}
