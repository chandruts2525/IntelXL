using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageOfInstructionsController : GenericController<LanguageOfInstructionMaster>
    {        
        private readonly IntelxlContext _context;
        private readonly ILogger<LanguageOfInstructionsController> _logger;

        public LanguageOfInstructionsController(ILogger<LanguageOfInstructionsController> logger,  IntelxlContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new LanguageOfInstructionMaster();
            try
            {
                response = await _context.LanguageOfInstructionMasters
                    .Include(c => c.CourseMasters)
                    .FirstOrDefaultAsync(c => c.LanguageId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(response);
        }
    }
}
