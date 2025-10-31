using TecWebFest.Api.DTOs;
using TecWebFest.Api.Entities;
using TecWebFest.Api.Repositories.Interfaces;
using TecWebFest.Api.Services.Interfaces;

namespace TecWebFest.Api.Services
{
    public class FestivalService : IFestivalService
    {
        private readonly IFestivalRepository _festivals;

        public FestivalService(IFestivalRepository festivals)
        {
            _festivals = festivals;
        }

        public async Task<int> CreateFestivalAsync(CreateFestivalDto dto)
        {
            var entity = new Festival
            {
                Name = dto.Name,
                City = dto.City,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                Stages = dto.Stages.Select(s => new Stage { Name = s.Name }).ToList()
            };

            await _festivals.AddAsync(entity);
            await _festivals.SaveChangesAsync();
            return entity.Id;
        }


        public async Task<FestivalLineupDto?> GetLineupAsync(int id)
        {
            var fest = await _festivals.GetLineupAsync(id);
            if (fest == null) return null;

            return new FestivalLineupDto
            {
                Festival = fest.Name,
                City = fest.City,
                Stages = fest.Stages
                    .OrderBy(s => s.Name)
                    .Select(s => new StageScheduleDto
                    {
                        Stage = s.Name,
                        Performances = s.Performances
                            .OrderBy(p => p.StartTime)
                            .Select(p => new PerformanceDto
                            {
                                ArtistId = p.ArtistId,
                                Artist = p.Artist.StageName,
                                StageId = p.StageId,
                                Stage = s.Name,
                                StartTime = p.StartTime,
                                EndTime = p.EndTime
                            }).ToList()
                    }).ToList()
            };
        }
    }
}