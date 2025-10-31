using Microsoft.EntityFrameworkCore;
using TecWebFest.Api.Data;
using TecWebFest.Api.Entities;
using TecWebFest.Api.Repositories.Interfaces;

namespace TecWebFest.Api.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        // TODO INYECCION 
        private readonly AppDbContext _ctx;
        public ArtistRepository(AppDbContext ctx) => _ctx = ctx;

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

        public async Task AddAsync(Artist artist)
        {
            await _ctx.Artists.AddAsync(artist);
        }

        public Task<Artist?> GetScheduleAsync(int id)
        {
            return _ctx.Artists
                .Include(a => a.Performances)
                    .ThenInclude(p => p.Stage)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public Task<bool> ExistsAsync(int id) =>
        _ctx.Artists.AnyAsync(a => a.Id == id);

    }
}
