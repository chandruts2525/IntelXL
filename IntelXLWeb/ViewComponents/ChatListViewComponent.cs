using Azure;

using IntelXL.HttpHandler;
using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IntelXLWeb.ViewComponents
{
    public class ChatListViewComponent : ViewComponent
    {
        private readonly ILogger<ChatListViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string chatsUri; 
        private readonly string? baseUri;
        public ChatListViewComponent(ILogger<ChatListViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            chatsUri = baseUri + IntelXlApiEnum.Chats;
        }
        public async Task<IViewComponentResult> InvokeAsync(int toUserId,bool isArchived)
        {
            List<Chat> chats = new ();
            ViewBag.ToUserId = toUserId;
            try
            {
                string endpoint = $"{chatsUri}/GetChatListById/" + toUserId;               
                if(isArchived)
                {
                    endpoint= $"{chatsUri}/GetArchivedChats/" + toUserId;
                }
              chats = await _httpHandler.GetAsync<List<Chat>>(endpoint);
                chats = chats.GroupBy(c => c.ConversationId)
                    .Select(group => group.Last())
                    .OrderByDescending(chat => chat.SentAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(chats);
        }
    }

    
}
