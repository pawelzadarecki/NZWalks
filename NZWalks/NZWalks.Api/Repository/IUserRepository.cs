using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
