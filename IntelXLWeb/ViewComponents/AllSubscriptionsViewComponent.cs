using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IntelXLWeb.ViewComponents
{
    public class AllSubscriptionsViewComponent : ViewComponent
    {
        private readonly ILogger<AllSubscriptionsViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string? _subscriptionsUri; 
        private readonly string? baseUri;
        public AllSubscriptionsViewComponent(ILogger<AllSubscriptionsViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            _subscriptionsUri = baseUri + IntelXlApiEnum.Subscription;
        }
        public async Task<IViewComponentResult> InvokeAsync(int courseId, int classId)
        {
            List<SubscriptionMaster> response = new List<SubscriptionMaster>();

            try
            {
                response = await _httpHandler.GetAsync<List<SubscriptionMaster>>(_subscriptionsUri + "/Subscriptions/" + classId);   
                //response=response.Where(r=>r.CourseId==courseId && r.ClassId==classId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }            
            return View(response);
        }

    }
}
