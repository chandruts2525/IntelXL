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
    public class TopicsController : Controller
    {
        private readonly IHttpHandler _httpHandler;
        private readonly string? topicsUri;
        private readonly string? unitsUri;
        private int _userId;
        private readonly string? baseUri;
        public TopicsController(IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            unitsUri = baseUri + IntelXlApiEnum.Units;
            topicsUri = baseUri + IntelXlApiEnum.Topics; 
            ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _userId = userId;
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllById(int id)
        {
            UnitMaster unit = new();
            string uri = $"{unitsUri}/GetListById/{id}";
            unit = await _httpHandler.GetAsync<UnitMaster>(uri);
            return Json(unit.TopicMasters);
        }
        public async Task<IActionResult> AddTopic(TopicMaster topicMaster)
        {
            topicMaster.CreatedDttm = DateTime.UtcNow;
            topicMaster.CreatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(topicMaster), Encoding.UTF8, "application/json");
            var result = await _httpHandler.PostAsync(topicsUri, stringContent);
            var responseContent = await result.Content.ReadAsStringAsync();
            TopicMaster topic = JsonConvert.DeserializeObject<TopicMaster>(responseContent);
            return Json(new { id = topic.TopicId, value = topic.Topic });
        }
        
        public async Task<bool> Update(TopicMaster topicMaster)
        {
            var uri = $"{topicsUri}/{topicMaster.TopicId}";
            var data = await _httpHandler.GetAsync<TopicMaster>(uri);
            data.Topic = topicMaster.Topic;
            data.CreatedDttm = DateTime.UtcNow;
            data.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(uri, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id)
        {
            TopicMaster topicMaster = new();
            topicMaster = await _httpHandler.GetAsync<TopicMaster>(topicsUri + "/" + id);
            topicMaster.Status = false;
            topicMaster.UpdatedDttm = DateTime.UtcNow;
            topicMaster.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(topicMaster), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(topicsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Restore(int id)
        {
            TopicMaster topicMaster = new();
            topicMaster = await _httpHandler.GetAsync<TopicMaster>(topicsUri + "/" + id);
            topicMaster.Status = true;
            topicMaster.UpdatedDttm = DateTime.UtcNow;
            topicMaster.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(topicMaster), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(topicsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<IActionResult> GetById(int id)
        {
            TopicMaster topic = new TopicMaster();
            topic = await _httpHandler.GetAsync<TopicMaster>(topicsUri + "/" + id);
            var result = await GetAllById(topic.UnitId);
            return result;
        }
    }
}
