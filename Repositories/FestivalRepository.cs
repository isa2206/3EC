using Microsoft.EntityFrameworkCore;
using TecWebFest.Api.Data;
using TecWebFest.Api.Entities;
using TecWebFest.Api.Repositories.Interfaces;

namespace TecWebFest.Api.Repositories
{
    public class FestivalRepository : IFestivalRepository
    {
        // TODO INYECCION 
        private readonly AppDbContext _ctx; 
        public FestivalRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddAsync(Festival festival)
        {
            await _ctx.Festivals.AddAsync(festival);
        }
        public Task<Festival?> GetLineupAsync(int id)
        {
            return _ctx.Festivals
                .Include(f => f.Stages)
                    .ThenInclude(s => s.Performances)
                        .ThenInclude(p => p.Artist)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
