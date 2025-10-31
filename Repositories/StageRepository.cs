using Microsoft.EntityFrameworkCore;
using TecWebFest.Api.Data;

namespace TecWebFest.Repositories
{
    public class StageRepository : IStageRepository
    {
        // TODO INYECCION 
        private readonly AppDbContext _ctx;
        public StageRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public Task<bool> ExistsAsync(int id) =>
            _ctx.Stages.AnyAsync(s => s.Id == id);
    }
}
