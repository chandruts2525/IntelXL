using IntelXLDataAccess.Data;
using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : GenericController<SubjectMaster>
    {
        private readonly IntelxlContext _context;
        private readonly ILogger<SubjectsController> _logger;
        private readonly IBaseRepo<SubjectMaster> _baseRepo;
        public SubjectsController(ILogger<SubjectsController> logger, IBaseRepo<SubjectMaster> baseRepo, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _baseRepo = baseRepo;
            _context = context;
        }
        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new SubjectMaster();
            try
            {
                response = await _context.SubjectMasters
                    .Include(c => c.UnitMasters)
                    .FirstOrDefaultAsync(s => s.SubjectId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(response);
            //var entity = await _baseRepo.GetByIdAsync(id, e => e.SubjectId == id, e => e.UnitMasters.Where(e => e.Status == true));
            //return Ok(entity);
        }

        [HttpGet("GetByIdWithSubTopics/{id}")]
        public async Task<IActionResult> GetByIdWithSubTopics(int id)
        {
            var subject = await _context.SubjectMasters
                .Include(c => c.Class)
                .Include(s => s.UnitMasters)
                .ThenInclude(u => u.TopicMasters)
                .ThenInclude(t => t.SubTopicMasters)
                .FirstOrDefaultAsync(c => c.SubjectId == id);

            return Ok(subject);
        }

        [HttpGet("GetByIdIncludeAllAsync/{id}")]
        public async Task<IActionResult> GetByIdIncludeAllAsync(int id)
        {
            var subject = new SubjectMaster();
            try
            {
                subject = await _context.SubjectMasters
                    .Include(c => c.UnitMasters)
                    .ThenInclude(c => c.TopicMasters)
                    .ThenInclude(c => c.SubTopicMasters)
                    .FirstOrDefaultAsync(c => c.SubjectId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Ok(subject);
        }

        [HttpGet("GetSubjectsByClassId/{id}")]
        public async Task<IActionResult> GetSubjectsByClassId(int id)
        {
            var entity = await _context.SubjectMasters
                .OrderBy(x => x.Order)
                .Where(s => s.ClassId == id && s.Status == true)
                .ToListAsync();
            return Ok(entity);
        }
    }
}
