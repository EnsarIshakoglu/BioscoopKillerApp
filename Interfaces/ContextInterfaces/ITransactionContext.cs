using Models;

namespace Interfaces.ContextInterfaces
{
    public interface ITransactionContext
    {
        void GetOccupiedSeats(AiringMovie airingMovie);
        void SaveReservation(Reservation reservation);
    }
}