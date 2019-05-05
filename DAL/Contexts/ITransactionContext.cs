using Models;

namespace DAL.Contexts
{
    public interface ITransactionContext
    {
        void AddOccupiedSeats(AiringMovie airingMovie);
        void SaveReservation(Reservation reservation);
    }
}