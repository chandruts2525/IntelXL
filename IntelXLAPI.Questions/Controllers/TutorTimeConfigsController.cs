using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorTimeConfigsController : GenericController<TutorTimingConfig>
    {
        private readonly ILogger<TutorTimeConfigsController> _logger;
        private readonly IntelxlContext _context;

        public TutorTimeConfigsController(ILogger<TutorTimeConfigsController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost("CreateTimeCofig")]
        public async Task<IActionResult> CreateTimeCofig(List<TutorTimingConfig> timingConfigs)
        {
            try
            {
                await _context.Set<TutorTimingConfig>().AddRangeAsync(timingConfigs);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetTutorAvailability/{tutorId}")]
        public async Task<ActionResult<List<TutorTimingConfig>>> GetTutorAvailability(int tutorId)
        {
            try
            {
                List<TutorTimingConfig> tutorAvailability = await _context.TutorTimingConfigs
                    .Include(t=>t.FromTime)
                    .Include(t=>t.ToTime)
                    .Include(t=>t.Day)
                    .Where(t=>t.TutorId == tutorId)
                    .ToListAsync(); 

                if (tutorAvailability == null || tutorAvailability.Count == 0)
                {
                    return NotFound($"No availability found for tutor with ID {tutorId}");
                }
                return Ok(tutorAvailability);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting tutor availability: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
