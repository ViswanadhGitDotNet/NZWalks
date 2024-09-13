using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task<Region> IRegionRepository.CreateAsync(Region region)
        {
           await dbContext.Regions.AddAsync(region);
           await dbContext.SaveChangesAsync();
            return region;
        }

        async Task<Region?> IRegionRepository.DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
              if(existingRegion==null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        async Task<List<Region>> IRegionRepository.GetAllAsync()
        {
          return await dbContext.Regions.ToListAsync();
        }

        async Task<Region?> IRegionRepository.GetByIdASync(Guid id)
        {
          return await  dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        async Task<Region?> IRegionRepository.UpdateAsync(Guid id, Region region)
        {
           var existingRegion=await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if(existingRegion==null)
            {
                return null;
            }
            existingRegion.Code= region.Code;
            existingRegion.Name= region.Name;
            existingRegion.RegionImageUrl= region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
