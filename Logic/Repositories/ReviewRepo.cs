using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace Logic.Repositories
{
    public class ReviewRepo
    {
        private readonly IReviewContext _context;

        public ReviewRepo(IReviewContext context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetAllReviewsFromMovie(Movie movie)
        {
            return _context.GetAllReviewsFromMovie(movie);
        }
        public void SaveReview(Review review)
        {
            _context.SaveReview(review);
        }
    }
}
