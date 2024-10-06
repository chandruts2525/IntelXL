using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IntelXLWeb.ViewComponents
{
    public class MessageListViewComponent : ViewComponent
    {
        private readonly ILogger<MessageListViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string chatsUri; 
        private readonly string? baseUri;
        public MessageListViewComponent(ILogger<MessageListViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            chatsUri = baseUri + IntelXlApiEnum.Chats;
        }
        public async Task<IViewComponentResult> InvokeAsync(int fromUserId,int toUserId)
        {  
            List<Chat> chats = new();
            ViewBag.ToUserId=toUserId;
            try
            {
                string endpoint = $"{chatsUri}/GetMessageList/{fromUserId}?toUserId={toUserId}";
                chats = await _httpHandler.GetAsync<List<Chat>>(endpoint);
                chats = chats.OrderBy(chat => chat.SentAt).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(chats);
        }
    }
}
