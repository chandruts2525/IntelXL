using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : GenericController<CouponMaster>
    {
        private readonly IntelxlContext _context;
        private readonly ILogger<CouponsController> _logger;

        public CouponsController(IntelxlContext context, ILogger<CouponsController> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetByCouponCode/{code}")]
        public async Task<IActionResult> GetByCouponCode(string code)
        {
            CouponMaster? coupon = new CouponMaster();
            try
            {
                coupon = await _context.CouponMasters.FirstOrDefaultAsync(c => c.CouponCode == code.ToUpper() && c.Status == true && c.EndDate >= DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(coupon);
        }
    }
}
