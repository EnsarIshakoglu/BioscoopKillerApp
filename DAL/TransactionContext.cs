using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interfaces;
using Models;

namespace DAL
{
    public class TransactionContext : ITransactionContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public void GetOccupiedSeats(AiringMovie airingMovie)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("GetOccupiedSeats", connection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@AiringMovieId", airingMovie.Id));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    airingMovie.Room.Seats.First(s => s.SeatNumber == (int)reader["SeatNumber"]).IsOccupied =
                        true;
                }

                connection.Close();
            }
        }

        public void SaveReservation(Reservation reservation)
        {
            foreach (var number in reservation.SeatNumbers)
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SaveReservation", connection) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.AddWithValue("@SeatNumber", Convert.ToInt32(number));
                    cmd.Parameters.AddWithValue("@AiringMovieID", Convert.ToInt32(reservation.AiringMovieId));
                    cmd.Parameters.AddWithValue("@Email", reservation.MailAddress);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
    }
}
