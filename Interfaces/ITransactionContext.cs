using Models;

namespace Interfaces
{
    public interface ITransactionContext
    {
        void AddOccupiedSeats(AiringMovie airingMovie);
        void SaveReservation(Reservation reservation);
    }
}