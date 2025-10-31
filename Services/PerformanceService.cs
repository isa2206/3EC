using TecWebFest.Api.DTOs;
using TecWebFest.Api.Entities;
using TecWebFest.Api.Repositories.Interfaces;
using TecWebFest.Api.Services.Interfaces;
using TecWebFest.Repositories;

namespace TecWebFest.Api.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IPerformanceRepository _performances;
        private readonly IStageRepository _stages;
        private readonly IArtistRepository _artists;

        public PerformanceService(
            IPerformanceRepository performances,
            IStageRepository stages,
            IArtistRepository artists)
        {
            _performances = performances;
            _stages = stages;
            _artists = artists;
        }

        public async Task AddPerformanceAsync(CreatePerformanceDto dto)
        {
            // PISTA TE PASO COMO EXTRAER START TIME Y END TIME EN FORMATO UTC
            //var startUtc = dto.StartTime.Kind == DateTimeKind.Unspecified
            //    ? DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc)
            //    : dto.StartTime.ToUniversalTime();

            //var endUtc = dto.EndTime.Kind == DateTimeKind.Unspecified
            //    ? DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc)
            //    : dto.EndTime.ToUniversalTime();

            // TODO VERIFICAR QUE EXISTA ARTISTA, STAGES Y QUE LA FECHA DE FIN SEA MAYOR QUE LA DEL INICIO


            // TODO VERIFICAR QUE NO HAYA SOLAPAMIENTO ENTRE PERFEROMANCES VER QUE EL STAGE ESTE LIBRE.

            // ANADIR EL PERFORMANCE
            // Validar que existe artista
            if (!await _artists.ExistsAsync(dto.ArtistId))
                throw new ArgumentException($"Artist with Id {dto.ArtistId} does not exist.");

            // Validar que existe stage
            if (!await _stages.ExistsAsync(dto.StageId))
                throw new ArgumentException($"Stage with Id {dto.StageId} does not exist.");

            // Validar que EndTime > StartTime
            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("EndTime must be greater than StartTime.");

            // Convertir a UTC
            var startUtc = dto.StartTime.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc)
                : dto.StartTime.ToUniversalTime();

            var endUtc = dto.EndTime.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc)
                : dto.EndTime.ToUniversalTime();

            // Verificar solapamiento
            bool overlap = await _performances.HasOverlapAsync(dto.StageId, startUtc, endUtc);
            if (overlap)
                throw new InvalidOperationException("There is already a performance in this stage during the specified time.");

            // Crear y guardar Performance
            var performance = new Performance
            {
                ArtistId = dto.ArtistId,
                StageId = dto.StageId,
                StartTime = startUtc,
                EndTime = endUtc
            };

            await _performances.AddAsync(performance);
            await _performances.SaveChangesAsync();
        }
    }
}
