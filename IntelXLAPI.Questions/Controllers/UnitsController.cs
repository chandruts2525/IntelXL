using IntelXLDataAccess.Data;
using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : GenericController<UnitMaster>
    {
        private readonly IBaseRepo<UnitMaster> _baseRepo;
        private readonly ILogger<UnitsController> _logger;
        private readonly IntelxlContext _context;

        public UnitsController(ILogger<UnitsController> logger, IBaseRepo<UnitMaster> baseRepo, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _baseRepo = baseRepo;
            _context = context;
        }


        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new UnitMaster();
            try
            {
                response = await _context.UnitMasters
                    .Include(c => c.TopicMasters)
                    .FirstOrDefaultAsync(s => s.UnitId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(response);           
        }

        [HttpGet("GetByIdIncludeAllAsync/{id}")]  
        public async Task<IActionResult> GetByIdIncludeAllAsync(int id)
        {
            var unit = new UnitMaster();
            try
            {
                unit = await _context.UnitMasters
                    .Include(c => c.TopicMasters)
                    .ThenInclude(c => c.SubTopicMasters)
                    .FirstOrDefaultAsync(c => c.UnitId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Ok(unit);
        }
    }
}
