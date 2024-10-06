using IntelXLDataAccess.Data;
using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : GenericController<CourseMaster>
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IBaseRepo<CourseMaster> _baseRepo;
        private readonly IntelxlContext _context;

        public CoursesController(ILogger<CoursesController> logger, IBaseRepo<CourseMaster> baseRepo, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _baseRepo = baseRepo;
            _context = context;
        }
        [HttpGet("GetAllCourse/{id}")]
        public async Task<IActionResult> GetAllCourse(int id)
        {
            IEnumerable<CourseMaster> courses = new List<CourseMaster>();
            try
            {
                courses = await _context.CourseMasters
                    .Where(c => c.LanguageId == id && c.Status == true)
                    .OrderBy(x => x.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(courses);
        }
        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new CourseMaster();
            try
            {
                response = await _context.CourseMasters
                    .Include(c => c.ClassMasters)
                    .FirstOrDefaultAsync(c => c.CourseId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(response);
        }

        [HttpGet("GetWithSubjectsAndTopics")]
        public async Task<IActionResult> GetWithSubjectsAndTopics([FromQuery] int id, [FromQuery] string subjectName)
        {
            //var course = new CourseMaster();
            try
            {
                var topicMasters = _context.SubjectMasters
                .Where(s => s.SubjectName == subjectName)
                .SelectMany(s => s.UnitMasters.SelectMany(u => u.TopicMasters)).Include(t => t.SubTopicMasters)
                .ToList();

                return Ok(topicMasters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetByIdIncludeAllAsync/{id}")]
        public async Task<IActionResult> GetByIdIncludeAllAsync(int id)
        {
            var course = new CourseMaster();
            try
            {
                course = await _context.CourseMasters
                    .Include(c => c.ClassMasters)
                    .ThenInclude(c => c.SubjectMasters.Where(s => s.Status))
                    .ThenInclude(c => c.UnitMasters.Where(s => s.Status))
                    .ThenInclude(c => c.TopicMasters.Where(s => s.Status))
                    .ThenInclude(c => c.SubTopicMasters.Where(s => s.Status))
                    .FirstOrDefaultAsync(c => c.CourseId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

            return Ok(course);
        }

        [HttpGet("GetAllIncludeSubject")]
        public async Task<IActionResult> GetAllIncludeSubject()
        {
            List<CourseMaster> courses = new List<CourseMaster>();
            try
            {
                courses = await _context.CourseMasters
                    .Include(c => c.ClassMasters)
                    .ThenInclude(c => c.SubjectMasters)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(courses);
        }

        [HttpGet("CoursesWithClasses/{languageId}")]
        public async Task<IActionResult> CoursesWithClasses(int languageId)
        {
            IEnumerable<CourseMaster> courses = new List<CourseMaster>();
            try
            {
                courses = await _context.CourseMasters
                    .Where(c => c.LanguageId == languageId && c.Status)
                    .OrderBy(x => x.Order)
                    .Include(c => c.ClassMasters
                    .Where(c => c.Status)
                    .OrderBy(c => c.Order))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(courses);
        }
    }
}
