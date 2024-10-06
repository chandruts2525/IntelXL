using IntelXLDataAccess.Data;
using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : GenericController<TopicMaster>
    {
        private readonly ILogger<TopicsController> _logger;
        private readonly IBaseRepo<TopicMaster> _baseRepo;
        private readonly IntelxlContext _context;
        private readonly int _subTopicsLimit;

        public TopicsController(ILogger<TopicsController> logger, IBaseRepo<TopicMaster> baseRepo, IConfiguration configuration, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _baseRepo = baseRepo;
            _context = context;
            _subTopicsLimit = configuration.GetValue<int>("SubTopicsLimit");
        }

        [HttpGet("GetListById/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new TopicMaster();
            try
            {
                response = await _context.TopicMasters
                    .Include(c => c.SubTopicMasters)
                    .FirstOrDefaultAsync(s => s.TopicId == id);
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
            var topic = new TopicMaster();
            try
            {
                topic = await _context.TopicMasters
                    .Include(c => c.SubTopicMasters)
                    .FirstOrDefaultAsync(c => c.TopicId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Ok(topic);
        }

        [HttpGet("GetTopicsWithSubTopicsBySubjectId/{id}")]
        public async Task<IActionResult> GetTopicsWithSubTopicsBySubjectId(int id)
        {
            var topics = new List<TopicMaster>();
            try
            {
                topics = await _context.SubjectMasters
                    .Where(s => s.SubjectId == id)
                    .SelectMany(s => s.UnitMasters)
                    .SelectMany(u => u.TopicMasters)
                    .Where(t => t.Status)
                    .OrderBy(t => t.Order)
                    .Include(t => t.SubTopicMasters)
                    .Select(t => new TopicMaster
                    {
                        TopicId = t.TopicId,
                        Topic = t.Topic,
                        SubTopicMasters = t.SubTopicMasters.Where(s => s.Status).OrderBy(s => s.Order).ToList(),
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Ok(new { subTopicLimit = _subTopicsLimit, Topics = topics });
        }
    }
}
