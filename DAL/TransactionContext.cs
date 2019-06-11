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
        private readonly string _dbConnectionString = "Server=mssql.fhict.local;Database=dbi419479;User Id=dbi419479;Password=Ensar123;";

        public void GetOccupiedSeats(AiringMovie airingMovie)
        {
            try
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
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }
        }

        public void SaveReservation(Reservation reservation)
        {
            foreach (var seat in reservation.Seats)
            {
                try
                {
                    using (var connection = new SqlConnection(_dbConnectionString))
                    {
                        connection.Open();

                        var cmd = new SqlCommand("SaveReservation", connection) { CommandType = CommandType.StoredProcedure };

                        cmd.Parameters.AddWithValue("@SeatNumber", seat.SeatNumber);
                        cmd.Parameters.AddWithValue("@AiringMovieID", reservation.AiringMovie.Id);
                        cmd.Parameters.AddWithValue("@Email", reservation.MailAddress);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //File.AppendAllText(, message);
                }
            }
        }
    }
}
