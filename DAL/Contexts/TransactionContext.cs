﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Models;

namespace DAL.Contexts
{
    public class TransactionContext : ITransactionContext
    {
        private readonly string _dbConnectionString = "Data Source=(LocalDb)\\DBMVCKillerAppTest;Initial Catalog = CinemaDB_2; Integrated Security = True";

        public void AddOccupiedSeats(AiringMovie airingMovie)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();

                var sqlCommand =
                    new SqlCommand(
                        $"SELECT UserID, SeatNumber FROM dbo.OccupiedSeats WHERE AiringMovieID = {airingMovie.Id}",
                        connection);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    airingMovie.Room.Seats.First(s => s.SeatNumber == (int) reader["SeatNumber"]).IsOccupied =
                        true;
                }

                connection.Close();
            }
        }
    }
}
