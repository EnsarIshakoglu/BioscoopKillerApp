using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IApiHelper
    {
        Task AddApiDataToMovie(Movie movie);
    }
}