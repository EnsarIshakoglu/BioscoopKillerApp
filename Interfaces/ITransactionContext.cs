using Models;

namespace Interfaces
{
    public interface ITransactionContext
    {
        void GetOccupiedSeats(AiringMovie airingMovie);
        void SaveReservation(Reservation reservation);
    }
}