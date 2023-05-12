using System.Threading.Tasks;
using OnlineStoreForWoman.Entities;

namespace OnlineStoreForWoman.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user, string existingToken = null);
    }
}