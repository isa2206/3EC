using Microsoft.AspNetCore.Mvc;
using TecWebFest.Api.DTOs;
using TecWebFest.Api.Services.Interfaces;

namespace TecWebFest.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ArtistsController : ControllerBase
    {
        //TODO INEYECCION DE DEPENDENCIAS - PISTA NECESITAS 2 INYECCIONES ARTIST Y PERFORMANCE
        private readonly IArtistService _artists;
        private readonly IPerformanceService _performance;

        public ArtistsController(
             IArtistService artists,
             IPerformanceService performance)
        {
            _artists = artists;
            _performance = performance;
        }

        // POST: api/v1/artists
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArtistDto dto)
        {
            var id = await _artists.CreateAsync(dto);
            return CreatedAtAction(nameof(GetSchedule), new { id }, null);
        }

        // GET: api/v1/artists/{id}/schedule
        [HttpGet("{id:int}/schedule")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var schedule = await _artists.GetScheduleAsync(id);

            if (schedule == null)
                return NotFound(new { error = "El artista no existe." });

            return Ok(schedule);
        }

        // POST: api/v1/artists/performances
        [HttpPost("performances")]
        public async Task<IActionResult> AddPerformance([FromBody] CreatePerformanceDto dto)
        {
            try
            {
                await _performance.AddPerformanceAsync(dto);
                return Ok(new { message = "Performance agregado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
