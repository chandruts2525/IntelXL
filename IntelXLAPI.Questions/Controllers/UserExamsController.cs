using IntelXLAdmin.Api.Controllers;
using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserExamsController : GenericController<UserExam>
    {
        private readonly IntelxlContext _context;
        private readonly ILogger<UserExamsController> _logger;
        public UserExamsController(ILogger<UserExamsController> logger, IntelxlContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpDelete("DeleteRange/{id}")]
        public async Task<IActionResult> DeleteRange(int id, [FromQuery] int userId)
        {
            try
            {
                var itemsToDelete = await _context.UserExams
                    .Where(item => item.AppUserId == userId && item.SubtopicId == id && item.PracticeType == "practice")
                    .ToListAsync();

                if (itemsToDelete.Any())
                {
                    _context.UserExams.RemoveRange(itemsToDelete);
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
        [HttpDelete("DeletePreviousRange/{id}")]
        public async Task<IActionResult> DeletePreviousRange(int id, [FromQuery] int userId, [FromQuery] string type, string year)
        {
            try
            {
                var itemsToDelete = await _context.UserExams
                    .Where(item => item.AppUserId == userId && item.SubjectId == id && item.PracticeType == type && item.YearOfQuestion == year)
                    .ToListAsync();

                if (itemsToDelete.Any())
                {
                    _context.UserExams.RemoveRange(itemsToDelete);
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
        [HttpGet("IsQuestionExists/{userId}")]
        public async Task<IActionResult> IsQuestionExists(int userId, int questionId, string type,string? year)
        {
            UserExam? userexam = new UserExam();
            try
            {
                userexam = await _context.UserExams
                    .FirstOrDefaultAsync(ue => ue.AppUserId == userId && ue.QuestionId == questionId && ue.PracticeType == type && ue.YearOfQuestion == year);
                if (userexam != null)
                    return Ok(userexam);
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

