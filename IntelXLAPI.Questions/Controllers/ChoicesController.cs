using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NLog.Targets;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : GenericController<ChoiceMaster>
    {
        private readonly IntelxlContext _context;

        public ChoicesController(IntelxlContext context) : base(context)
        {
            _context = context;
        }
        [HttpDelete("DeleteRange/{id}")]
        public async Task<IActionResult> DeleteRange(int id)
        {
            try
            {
                var itemsToDelete = await _context.ChoiceMasters
                    .Where(item => item.QuestionId == id)
                    .ToListAsync();

                if (itemsToDelete.Any())
                {
                    _context.ChoiceMasters.RemoveRange(itemsToDelete);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound("No items found to delete.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
