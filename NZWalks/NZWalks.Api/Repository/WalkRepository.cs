using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public class WalkRepository: IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks.
                Include(x => x.Region).
                Include(x => x.WalkDifficulty).
                ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks.
                Include(x => x.Region).
                Include(x => x.WalkDifficulty).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = new Guid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //find object in database
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;

            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            //check by id
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //Check if exist
            if(walk == null)
            {
                return null;
            }

            //delete walk
            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();

            //return respond
            return walk;
        }
    }
}
