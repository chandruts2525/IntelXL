using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : GenericController<Chat>
    {
        private readonly ILogger<ChatsController> _logger;
        private readonly IntelxlContext _context;

        public ChatsController(ILogger<ChatsController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetChatListById/{id}")]
        public async Task<IActionResult> GetChatListById(int id)
        {
            List<Chat> chatList = new();
            try
            {
                chatList = await _context.Chats
                    .Include(c => c.FromUser)
                    .Include(c => c.ToUser)
                    .Where(c => (c.ToId == id || c.FromId == id) && !c.IsArchived)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(chatList);
        }

        [HttpGet("GetByToId/{id}")]
        public async Task<IActionResult> GetByToId(int id)
        {
            List<Chat> chatList = new();
            try
            {
                chatList = await _context.Chats
                    .Where(c => c.ToId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(chatList);
        }
        [HttpGet("GetMessageList/{fromUserId}")]
        public async Task<IActionResult> GetMessageList(int fromUserId, [FromQuery] int toUserId)
        {
            List<Chat> messages = new();
            try
            {
                messages = await _context.Chats
                    .Where(c => c.ToId == toUserId && c.FromId == fromUserId || c.ToId == fromUserId && c.FromId == toUserId)
                    //.Where(c => c.ConversationId == fromUserId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(messages);
        }

        [HttpPut("UpdateDeliverStatus")]
        public async Task<IActionResult> UpdateDeliverStatus(List<Chat> chats)
        {
            try
            {
                _context.Chats.UpdateRange(chats);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetUnreadCount/{id}")]
        public async Task<IActionResult> GetUnreadCount(int id)
        {
            int count = 0;
            try
            {
                count = await _context.Chats
                    .CountAsync(c => c.ToId == id && !c.IsRead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(count);
        }
        [HttpPut("UpdateReadStatus")]
        public async Task<IActionResult> UpdateReadStatus(List<Chat> chats)
        {
            try
            {
                _context.Chats.UpdateRange(chats);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetArchivedChats/{id}")]
        public async Task<IActionResult> GetArchivedChats(int id)
        {
            List<Chat> chatList = new();
            try
            {
                chatList = await _context.Chats
                    .Include(c => c.FromUser)
                    .Include(c => c.ToUser)
                    .Where(c => (c.ToId == id || c.FromId == id) && c.IsArchived)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(chatList);
        }
    }
}
