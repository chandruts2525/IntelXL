using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : GenericController<AppUser>
    {
        private readonly IntelxlContext _context;
        private readonly ILogger<QuestionsController> _logger;
        private readonly int pageSize;
        public UserController(ILogger<QuestionsController> logger, IntelxlContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        [HttpGet("GetAllUsers/{pageNum}")]
        public async Task<IActionResult> GetAllUsers(int pageNum)
        {
            IEnumerable<AppUser> users = new List<AppUser>();
            int totalPages = 0;
            try
            {
                var allItems = _context.AppUsers.AsQueryable();
                totalPages = (int)Math.Ceiling(await allItems.CountAsync() / (double)pageSize);
                users = await allItems
                   .Include(c => c.AppRole).Include(s => s.UserSubscriptions)
                    //.OrderByDescending(p => p.CreatedDttm)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { TotalPages = totalPages, Users = users, PageSize = pageSize });
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            AppUser? user = new();
            user = await _context.AppUsers
                .Include(u => u.TutorDetails)
            .Include(u => u.UserSubscriptions)
            .ThenInclude(u => u.Subscription)
            .Include(u => u.UserPayments)
            .FirstOrDefaultAsync(u => u.AppUserId == id);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpGet("IsUserExist/{email}")]
        public async Task<IActionResult> IsUserExist(string email)
        {
            var user = await _context.AppUsers.Include(c => c.AppRole)
                .FirstOrDefaultAsync(c => c.EmailId == email);
            if (user != null)
                return Ok(user);
            else
                return NotFound(user);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string userId, string password)
        {
            try
            {
                var result = await _context.AppUsers
       .Include(c => c.AppRole)
       .Include(c => c.UserLogin)
       .Include(u => u.UserSubscriptions.Where(u => u.Status == true && u.ExpireDttm >= DateTimeOffset.Now))
       .ThenInclude(us => us.Subscription)
       .Where(c => c.EmailId == userId && c.Password == password && c.Status == true)
       .FirstOrDefaultAsync();
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        [HttpPut("UpdateUserData/{id}")]
        public async Task<IActionResult> UpdateUserData(int id, AppUser user)
        {
            try
            {
                var userDbData = await _context.AppUsers.FindAsync(id);

                if (userDbData == null)
                    return NotFound();

                if (!string.IsNullOrEmpty(user.FirstName))
                    userDbData.FirstName = user.FirstName;
                if (!string.IsNullOrEmpty(user.LastName))
                    userDbData.LastName = user.LastName;
                if (!string.IsNullOrEmpty(user.EmailId))
                    userDbData.EmailId = user.EmailId;
                if (!string.IsNullOrEmpty(user.Password))
                    userDbData.Password = user.Password;

                userDbData.UpdatedBy = user.AppUserId;
                userDbData.UpdatedDttm = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return BadRequest(new { Message = "Failed to update user." });
            }
        }

        [HttpGet("GetTutorById/{id}")]
        public async Task<IActionResult> GetTutorById(int id)
        {
            AppUser? tutor = new();
            try
            {
                tutor = await _context.AppUsers
                    .Include(t => t.TutorDetails)
                    .FirstOrDefaultAsync(t => t.AppUserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(tutor);
        }
        [HttpGet("SearchUser/{pageNum}")]
        public async Task<IActionResult> SearchUser(int pageNum, string searchTerm)
        {
            IEnumerable<AppUser> users = new List<AppUser>();
            int totalPages = 0;
            try
            {
                var allItems = _context.AppUsers.AsQueryable();
                totalPages = (int)Math.Ceiling(await allItems.CountAsync(u => u.EmailId.Contains(searchTerm) || u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm)) / (double)pageSize);
                users = await allItems
                    .Include(c => c.AppRole)
                    .Include(s => s.UserSubscriptions)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .Where(u => u.EmailId.Contains(searchTerm) || u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { TotalPages = totalPages, Users = users });
        }
    }
}
