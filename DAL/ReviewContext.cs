using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Interfaces;
using Interfaces.ContextInterfaces;
using Models;

namespace DAL
{
    public class ReviewContext : IReviewContext
    {
        private readonly string _dbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";

        public IEnumerable<Review> GetAllReviewsFromMovie(Movie movie)
        {
            var reviews = new List<Review>();

            try
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var cmd = new SqlCommand("GetReviewsFromMovie", connection) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@MovieId", movie.Id));

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        reviews.Add(new Review
                        {
                            Movie = new Movie
                            {
                                Id = (int)reader["MovieID"]
                            },
                            ReviewText = reader["ReviewText"]?.ToString(),
                            ReviewTitle = reader["ReviewTitle"]?.ToString(),
                            User = new User
                            {
                                Id = (int)reader["UserID"],
                                Name = reader["Name"]?.ToString()
                            },
                            PostedDate = (DateTime)reader["PostedDate"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }

            return reviews;
        }
        public void SaveReview(Review review)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    var sqlCommand = new SqlCommand($"SaveReview", connection) { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                    sqlCommand.Parameters.AddWithValue("@ReviewTitle", review.ReviewTitle);
                    sqlCommand.Parameters.AddWithValue("@MovieId", review.Movie.Id);
                    sqlCommand.Parameters.AddWithValue("@UserId", review.User.Id);
                    sqlCommand.Parameters.AddWithValue("@PostedDate", DateTime.Today.Date);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }
        }
    }
}
