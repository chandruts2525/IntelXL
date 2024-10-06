using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : GenericController<SubscriptionMaster>
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IntelxlContext _context;
        public SubscriptionController(ILogger<SubscriptionController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetAllSubscriptions")]
        public async Task<IActionResult> GetAllSubscription()
        {
            IEnumerable<SubscriptionMaster> subscriptions = new List<SubscriptionMaster>();
            try
            {
                subscriptions = await _context.SubscriptionMasters
                    .Where(q => q.Status == true)
                    .Include(q => q.Course)
                    .Include(q => q.Class)
                    .ThenInclude(q => q.SubjectMasters)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Ok(subscriptions);
        }
        
        [HttpGet("Subscriptions/{classId}")]
        public async Task<IActionResult> Subscriptions(int classId)
        {
            IEnumerable<SubscriptionMaster> subscriptions = new List<SubscriptionMaster>();
            try
            {
                subscriptions = await _context.SubscriptionMasters
                    .Where(s => s.Status == true && s.ClassId == classId)
                    .Include(q => q.Course)
                    .Include(q => q.Class)
                    .ThenInclude(q => q.SubjectMasters)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Ok(subscriptions);
        }
    }
}
