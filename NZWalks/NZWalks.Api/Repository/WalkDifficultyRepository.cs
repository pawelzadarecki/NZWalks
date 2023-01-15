using NZWalks.Api.Data;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
          return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            //get by id
           return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            // create ID number
            walkDifficulty.ID = new Guid();

            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;  
        }

        public async Task<WalkDifficulty> UpdateAsync(WalkDifficulty walkDifficulty, Guid id)
        {
            //Find data id database
            var existinfWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);

            //Check if null
            if(existinfWalkDifficulty == null)
            {
                return null;
            }

            //Updated 
            existinfWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();

            //Return result
            return existinfWalkDifficulty;

        }

        
        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            //find data in database
            var walkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.ID == id);

            //check if null
            if (walkDifficulty == null) return null;

            //Delete from database
            nZWalksDbContext.WalkDifficulty.Remove(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();

            //return result
            return walkDifficulty;
        }
    }
}
