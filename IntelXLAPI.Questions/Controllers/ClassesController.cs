using IntelXLDataAccess.Data;
using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : GenericController<ClassMaster>
    {
        private readonly ILogger<ClassesController> _logger;
        private readonly IBaseRepo<ClassMaster> _baseRepo;
        private readonly IntelxlContext _context;
        public ClassesController(ILogger<ClassesController> logger, IBaseRepo<ClassMaster> baseRepo, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _baseRepo = baseRepo;
            _context = context;
        }

        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new ClassMaster();
            try
            {
                response = await _context.ClassMasters
                    .Include(c => c.SubjectMasters)
                    .FirstOrDefaultAsync(c => c.ClassId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(response);
        }

        [HttpGet("GetAllWithSubTopics/{id}")]
        public async Task<IActionResult> GetAllWithTopicsCount(int id)
        {
            var course = await _context.CourseMasters
                .Include(c => c.ClassMasters.Where(cm => cm.Status == true))
                .ThenInclude(c => c.SubjectMasters.Where(sm => sm.Status == true))
                .ThenInclude(s => s.UnitMasters)
                .ThenInclude(u => u.TopicMasters)
                .ThenInclude(t => t.SubTopicMasters)
                .ThenInclude(t => t.QuestionMasters)
                .FirstOrDefaultAsync(c => c.CourseId == id);


            course?.ClassMasters?.Where(cm => cm.Status == true).ToList().ForEach(classMaster =>
            {
                classMaster.TotalQuestionsCount = 0;
                classMaster.SubjectMasters?.Where(subject => subject.Status == true).ToList().ForEach(subject =>
                {
                    subject.subTopicCount = 0;

                    subject.UnitMasters?.ToList().ForEach((unit) =>
                    {
                        unit.TopicMasters?.ToList().ForEach((topic) =>
                        {
                            subject.subTopicCount += topic.SubTopicMasters?.Count() ?? 0;
                            topic.SubTopicMasters?.ToList().ForEach(SubTopicMasters =>
                            {
                                classMaster.TotalQuestionsCount += SubTopicMasters?.QuestionMasters?.Count() ?? 0;
                            });
                        });
                    });

                    subject.UnitMasters?.Clear();
                });
            });

            return Ok(course?.ClassMasters);
        }

        [HttpGet("GetByIdIncludeAllAsync/{id}")]
        public async Task<IActionResult> GetByIdIncludeAllAsync(int id)
        {
            var classMaster = new ClassMaster();
            try
            {
                classMaster = await _context.ClassMasters
                    .Include(c => c.SubjectMasters)
                    .ThenInclude(c => c.UnitMasters)
                    .ThenInclude(c => c.TopicMasters)
                    .ThenInclude(c => c.SubTopicMasters)
                    .FirstOrDefaultAsync(c => c.ClassId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

            return Ok(classMaster);
        }

        [HttpGet("GetClassesByCourseId/{id}")]
        public async Task<IActionResult> GetClassesByCourseId(int id)
        {
            var entity = await _context.ClassMasters
                .OrderBy(x => x.Order)
                .Where(c => c.CourseId == id && c.Status == true)
                .ToListAsync();
            return Ok(entity);
        }
    }
}
