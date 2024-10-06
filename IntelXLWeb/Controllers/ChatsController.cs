using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;
using IntelXLWeb.Utilities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Text;

namespace IntelXLWeb.Controllers
{
    public class ChatsController : Controller
    {
        private readonly IHttpHandler _handler;
        private readonly string chatsUri;
        private readonly ILogger<ChatsController> _logger;
        private int userId;
        private readonly FireBaseStorageConfig _firebaseConfig;
        private readonly string? baseUri;
        public ChatsController(ILogger<ChatsController> logger, IHttpHandler handler, IHttpContextAccessor httpContextAccessor, IOptions<FireBaseStorageConfig> firebaseConfig, IConfiguration configuration)
        {
            _handler = handler;           
            baseUri = configuration.GetValue<string>("baseUrl");
            chatsUri = baseUri+IntelXlApiEnum.Chats; 
            _logger = logger;
            _firebaseConfig = firebaseConfig.Value;
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "UserID");
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }
        }
        public IActionResult GetChatsList(bool isArchived)
        {
            return ViewComponent("ChatList", new { toUserId = userId, isArchived = isArchived });
        }
        public IActionResult GetMessages(int fromUserId)
        {
            return ViewComponent("MessageList", new { fromUserId = fromUserId, toUserId = userId });
        }
        public async Task<int> GetUnreadCount()
        {
            int count = 0;
            try
            {
                HttpResponseMessage response = await _handler.GetAsync(chatsUri + "/GetUnreadCount/" + userId);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    count = int.Parse(responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return count;
        }
        public async Task<bool> UpdateReadStatus(int fromId)
        {
            bool result = false;
            List<Chat> chats = new List<Chat>();
            try
            {
                chats = await _handler.GetAsync<List<Chat>>($"{chatsUri}/GetMessageList/{fromId}?toUserId={userId}");
                if (chats?.Any(chat => chat.IsRead == false) == true)
                {
                    foreach (var chat in chats)
                    {
                        chat.IsRead = true;
                    }
                    string endpoint = $"{chatsUri}/UpdateReadStatus";
                    var stringContent = new StringContent(JsonConvert.SerializeObject(chats), Encoding.UTF8, "application/json");
                    var response = await _handler.PutAsync(endpoint, stringContent);
                    result = response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }

        public async Task<string> UploadAttachments(IFormFile file)
        {
            string filePath = string.Empty;
            try
            {
                //filePath = "https://firebasestorage.googleapis.com/v0/b/myproject-56e06.appspot.com/o/TutorProfiles%2Fwarning.png?alt=media&token=bd479a5d-4f44-4d87-ac4c-431b1f90964d";
                if (file != null && file.Length > 0)
                {
                   filePath = await StorageHelper.UploadFileToStorage(file, _firebaseConfig);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return filePath;
        }
        public IActionResult GetArchivedChats()
        {
            return ViewComponent("ChatList", new { toUserId = userId, isArchived = true });
        }
        public async Task<bool> AddToArchived(int fromId)
        {
            bool result = false;
            List<Chat> chats = new List<Chat>();
            try
            {
                chats = await _handler.GetAsync<List<Chat>>($"{chatsUri}/GetMessageList/{fromId}?toUserId={userId}");
                if (chats?.Any(chat => chat.IsArchived == false) == true)
                {
                    foreach (var chat in chats)
                    {
                        chat.IsArchived = true;
                    }                   
                    var stringContent = new StringContent(JsonConvert.SerializeObject(chats), Encoding.UTF8, "application/json");
                    var response = await _handler.PutAsync(chatsUri, stringContent);
                    result = response.IsSuccessStatusCode;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<bool> RemoveFromArchived(int fromId)
        {
            bool result = false;
            List<Chat> chats = new List<Chat>();
            try
            {
                chats = await _handler.GetAsync<List<Chat>>($"{chatsUri}/GetMessageList/{fromId}?toUserId={userId}");
                if (chats?.Any(chat => chat.IsArchived == true) == true)
                {
                    foreach (var chat in chats)
                    {
                        chat.IsArchived = false;
                    }                   
                    var stringContent = new StringContent(JsonConvert.SerializeObject(chats), Encoding.UTF8, "application/json");
                    var response = await _handler.PutAsync(chatsUri, stringContent);
                    result = response.IsSuccessStatusCode;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
    }
}
