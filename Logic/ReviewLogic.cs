using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Logic.Repositories;
using Models;

namespace Logic
{
    public class ReviewLogic
    {
        private readonly ReviewRepo _repo = new ReviewRepo(new ReviewContext());

        public IEnumerable<Review> GetAllReviewsFromMovie(Movie movie)
        {
            return _repo.GetAllReviewsFromMovie(movie);
        }
        public void SaveReview(Review review)
        {
            _repo.SaveReview(review);
        }
    }
}
