using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTopicsController : GenericController<SubTopicMaster>
    {
        private readonly ILogger<SubTopicsController> _logger;
        private readonly IntelxlContext _context;

        public SubTopicsController(ILogger<SubTopicsController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetByIdIncludeAllAsync/{id}")]
        public async Task<IActionResult> GetByIdIncludeAllAsync(int id)
        {
            var subTopic = new SubTopicMaster();
            try
            {
                subTopic = await _context.SubTopicMasters
                    .Include(c => c.QuestionMasters)
                        .ThenInclude(c => c.Answer)
                    .Include(c => c.QuestionMasters)
                        .ThenInclude(c => c.ChoiceMasters)
                    .FirstOrDefaultAsync(c => c.TopicId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Ok(subTopic);
        }

        //[HttpGet("GetTopicbySubtopic/{id}")]
        //public async Task<IActionResult> GetTopicbySubtopic(int id)
        //{
        //    var subTopic = new SubTopicMaster();
        //    try
        //    {
        //        subTopic = await _context.SubTopicMasters
        //            .Include(t=>t.Topic)
        //            .ThenInclude(u=>u.Unit)
        //            .ThenInclude(s=>s.Subject)
        //            .ThenInclude(cl=>cl.Class)
        //            .ThenInclude(c=>c.Course)
        //            .ThenInclude(l=>l.LanguageOfInstruction)                        
        //            .FirstOrDefaultAsync(st => st.SubTopicId == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message.ToString());
        //    }

        //    return Ok(subTopic);
        //}
    }
}
