using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
