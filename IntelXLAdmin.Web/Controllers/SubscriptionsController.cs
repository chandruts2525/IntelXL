using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class SubscriptionsController : Controller
    {       
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string _subscriptionsUri;
        private int _userId;
        private readonly string? baseUri;
        public SubscriptionsController(ILogger<SubscriptionsController> logger, IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            _subscriptionsUri = baseUri + IntelXlApiEnum.Subscription;
            ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _userId = userId;
            }
        }

        public async Task<IActionResult> Index()
        {
            List<SubscriptionMaster> response = new List<SubscriptionMaster>();
            try
            {
                response = await _httpHandler.GetAsync<List<SubscriptionMaster>>(_subscriptionsUri + "/GetAllSubscriptions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(response);           
        }

        public async Task<bool> AddSubscription(SubscriptionMaster subscription)
        {
          bool result = false;
            try
            {
                subscription.CreatedDttm = DateTime.Now;
                subscription.CreatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(subscription), Encoding.UTF8, "application/json");
                var response = await _httpHandler.PostAsync(_subscriptionsUri, stringContent);
                result = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }          
            return result;           
        }
        public async Task<bool> EditSubscription(SubscriptionMaster subscription)
        {
          bool result = false;
            try
            {
                subscription.UpdatedDttm = DateTime.Now;
                subscription.UpdatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(subscription), Encoding.UTF8, "application/json");
                var response = await _httpHandler.PutAsync(_subscriptionsUri + "/" + subscription.SubscriptionId, stringContent);
                result = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            SubscriptionMaster subscription = new();
            bool response = false;
            try
            {
                subscription = await _httpHandler.GetAsync<SubscriptionMaster>(_subscriptionsUri + "/" + id);
                subscription.Status = false;
                subscription.UpdatedDttm = DateTime.Now;
                subscription.UpdatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(subscription), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(_subscriptionsUri + "/" + id, stringContent);
                response = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
    }
}
