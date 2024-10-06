using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubscriptionsController : GenericController<UserSubscription>
    {
        private readonly IntelxlContext _context;
        private readonly ILogger<UserSubscriptionsController> _logger;
        public UserSubscriptionsController(ILogger<UserSubscriptionsController> logger, IntelxlContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet("ValidateSubscription/{userId}")]
        public async Task<IActionResult> ValidateSubscription(int userId,int subscriptionId)
        {
            UserSubscription? userSubscription = new UserSubscription();
            try
            {
                userSubscription = await _context.UserSubscriptions
                    .FirstOrDefaultAsync(us => us.AppUserId == userId && us.Status && us.SubscriptionId== subscriptionId);
                if (userSubscription != null)
                    return Ok(userSubscription);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }           
        }

    }
}
