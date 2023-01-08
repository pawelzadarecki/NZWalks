using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
