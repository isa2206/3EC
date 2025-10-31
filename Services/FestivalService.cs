using TecWebFest.Api.DTOs;
using TecWebFest.Api.Entities;
using TecWebFest.Api.Repositories.Interfaces;
using TecWebFest.Api.Services.Interfaces;

namespace TecWebFest.Api.Services
{
    public class FestivalService : IFestivalService
    {
        //TODO INEYECCION DE DEPENDENCIAS
        private readonly IFestivalRepository _repo;
        public FestivalService(IFestivalRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> CreateFestivalAsync(CreateFestivalDto dto)
        {
            //TODO
            //pista para importar stages: Stages = dto.Stages.Select(s => new Stage { Name = s.Name }).ToList()
            var festival = new Festival
            {
                Name = dto.Name,
                City = dto.City,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,

                Stages = dto.Stages
                .Select(s => new Stage { Name = s.Name })
                .ToList()
            };

            await _repo.AddAsync(festival);
            await _repo.SaveChangesAsync();

            return festival.Id;
        }


        public async Task<FestivalLineupDto?> GetLineupAsync(int id)
        {
            //TODO
            var festival = await _repo.GetLineupAsync(id);
            if (festival == null)
                return null;

            return new FestivalLineupDto
            {
                Festival = festival.Name,
                City = festival.City,
                Stages = festival.Stages
                    .Select(s => new StageScheduleDto
                    {
                        Stage = s.Name,
                        Performances = s.Performances
                            .OrderBy(p => p.StartTime)
                            .Select(p => new PerformanceDto
                            {
                                ArtistId = p.ArtistId,
                                Artist = p.Artist.StageName,
                                StageId = s.Id,
                                Stage = s.Name,
                                StartTime = p.StartTime,
                                EndTime = p.EndTime
                            }).ToList()
                    }).ToList()
            };
        }
    }
}

