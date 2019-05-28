using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IReviewContext
    {
        IEnumerable<Review> GetAllReviewsFromMovie(Movie movie);
        void SaveReview(Review review);
    }
}