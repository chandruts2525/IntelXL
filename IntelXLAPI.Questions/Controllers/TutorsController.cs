using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NLog.Targets;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorsController : GenericController<AppUser>
    {
        private readonly ILogger<TutorsController> _logger;
        private readonly IntelxlContext _context;

        public TutorsController(ILogger<TutorsController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetTutorById/{id}")]
        public async Task<IActionResult> GetTutorById(int id)
        {
            AppUser? tutor = new();
            try
            {
                tutor = await _context.AppUsers
                    .Include(t => t.TutorDetails)
                    .Include(t => t.StudentTutorScheduleAppUsers)                   
                    .FirstOrDefaultAsync(user => user.AppRole != null && user.AppRole.RoleName == "Tutor" && user.AppUserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(tutor);
        }
        [HttpGet("GetVerifiedTutors")]
        public async Task<IActionResult> GetVerifiedTutors()
        {
            List<AppUser> tutorList = new();
            try
            {
                tutorList = await _context.AppUsers.
                    Include(t => t.TutorDetails)
                    .Where(user => user.AppRole != null && user.AppRole.RoleName == "Tutor" && user.IsVerified == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(tutorList);
        }
        [HttpGet("GetAllUnverifiedTutors")]
        public async Task<IActionResult> GetAllUnverifiedTutors()
        {
            List<AppUser> tutorList = new();
            try
            {
                tutorList = await _context.AppUsers
                    .Include(t => t.TutorDetails)
                   .Where(user => user.AppRole != null && user.AppRole.RoleName == "Tutor" && user.IsVerified == false)
                   .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(tutorList);
        }
    }
}
