using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Text;

namespace ChatCore.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static Dictionary<int, string> users = new Dictionary<int, string>();
        private readonly IHttpHandler _httpHandler;
        private readonly ILogger<ChatHub> _logger;
        private readonly string chatsUri;
        private int userId;
        private readonly string? baseUri;
        public ChatHub(ILogger<ChatHub> logger, IHttpHandler handler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = handler;
            _logger = logger;
            chatsUri = baseUri + IntelXlApiEnum.Chats;
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "UserID");
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                string connectionId = Context.ConnectionId;
                users.Add(userId, connectionId);
                if (userId != 0)
                {
                    List<Chat> chats = new();
                    chats = await _httpHandler.GetAsync<List<Chat>>(chatsUri + "/GetByToId/" + userId);
                    if (chats?.Any(chat => chat.IsDelivered == false) == true)
                    {
                        foreach (var chat in chats)
                        {
                            chat.IsDelivered = true;
                        }
                        string endpoint = $"{chatsUri}/UpdateDeliverStatus";
                        var stringContent = new StringContent(JsonConvert.SerializeObject(chats), Encoding.UTF8, "application/json");
                        var response = await _httpHandler.PutAsync(endpoint, stringContent);
                    }
                }
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            users.Remove(userId);
            await base.OnDisconnectedAsync(exception);
        }
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        public async Task SendToUser(int toUserId, string message, string conversationId, string fileName, string fileUrl)
        {
            bool isDeliverd = false;
            if (users.ContainsKey(toUserId))
            {
                isDeliverd = true;
            }
            if (string.IsNullOrEmpty(conversationId))
            {
                conversationId = Guid.NewGuid().ToString();
            }
            Chat chatModel = new Chat
            {
                FromId = userId,
                ToId = toUserId,
                Message = message,
                ConversationId = conversationId,
                SentAt = DateTime.UtcNow,
                IsDelivered = isDeliverd,
                FileName= fileName,
                MediaUrl = fileUrl
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(chatModel), Encoding.UTF8, "application/json");
            await _httpHandler.PostAsync((chatsUri), stringContent);
            if (users.ContainsKey(toUserId))
            {
                string connectionId = users[toUserId];
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", chatModel);
            }
        }

        public async Task TypingIndication(int toUserId,string message)
        {          
            if (users.ContainsKey(toUserId))
            {
                string connectionId = users[toUserId];
                await Clients.Client(connectionId).SendAsync("TypingIndication", userId,message);
            }
        }
    }
}
