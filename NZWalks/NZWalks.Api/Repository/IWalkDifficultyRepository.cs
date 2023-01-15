using NZWalks.Api.Models.Domian;
namespace NZWalks.Api.Repository
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);
        Task<WalkDifficulty> UpdateAsync(WalkDifficulty walkDifficulty, Guid id);
        Task<WalkDifficulty> DeleteAsync(Guid id);
    }
}
