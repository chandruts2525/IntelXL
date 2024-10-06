using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : GenericController<UserPayment>
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IntelxlContext _context;
        private readonly IConfiguration _configuration;
        private readonly int pageSize;

        public PaymentsController(ILogger<PaymentsController> logger, IntelxlContext context, IConfiguration configuration) : base(context)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            pageSize = _configuration.GetValue<int>("PageSize");
        }
        [HttpGet("GetAllPayments/{pageNum}")]
        public async Task<IActionResult> GetAllPayments(int pageNum)
        {
            IEnumerable<UserPayment> payments = new List<UserPayment>();
            int totalPages = 0;
            try
            {
                var allItems = _context.UserPayments.AsQueryable();
                totalPages = (int)Math.Ceiling(await allItems.CountAsync() / (double)pageSize);
                payments = await allItems
                    .Include(u => u.AppUser)
                    .OrderByDescending(p => p.PaymentDate)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { TotalPages = totalPages, Payments = payments });
        }
        [HttpGet("GetDetailsById/{id}")]
        public async Task<IActionResult> GetDetailsById(int id)
        {
            UserPayment? userPaymentDetails = new();
            try
            {
                userPaymentDetails = await _context.UserPayments
                    .Include(p => p.AppUser)
                    .ThenInclude(p => p.UserSubscriptions)
                    .ThenInclude(u => u.Subscription)
                    .FirstOrDefaultAsync(p => p.PaymentId == id);

                if (userPaymentDetails != null)
                {
                    return Ok(userPaymentDetails);
                }
                else
                {
                    return NotFound($"Payment details with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
