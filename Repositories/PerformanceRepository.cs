using Microsoft.EntityFrameworkCore;
using TecWebFest.Api.Data;
using TecWebFest.Api.Entities;

namespace TecWebFest.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        // TODO INYECCION 
        private readonly AppDbContext _ctx;
        public PerformanceRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddAsync(Performance performance)
        {
            await _ctx.Performances.AddAsync(performance);
        }
        public Task<bool> HasOverlapAsync(int stageId, DateTime start, DateTime end)
        {
            return _ctx.Performances
                .AnyAsync(p => p.StageId == stageId
                               && start < p.EndTime
                               && end > p.StartTime);
        }

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
