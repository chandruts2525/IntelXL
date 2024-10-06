using IntelXL.HttpHandler;
using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    public class SubTopicsController : Controller
    {
        private readonly IHttpHandler _httpHandler;
        private readonly string? subtopicsUri;
        private readonly string? topicsUri;
        private readonly ILogger<SubTopicsController> _logger; 
        private int _userId;
        private readonly string? baseUri;
        public SubTopicsController(IHttpHandler httpHandler, ILogger<SubTopicsController> logger, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;           
            topicsUri = baseUri + IntelXlApiEnum.Topics;
            subtopicsUri = baseUri + IntelXlApiEnum.SubTopics;
            _logger = logger;
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
            TopicMaster topic = new();
            string uri = $"{topicsUri}/GetListById/{id}";
            topic = await _httpHandler.GetAsync<TopicMaster>(uri);
            return Json(topic.SubTopicMasters);
        }
        public async Task<IActionResult> AddSubTopic(SubTopicMaster subTopicMaster)
        {
            subTopicMaster.CreatedDttm = DateTime.UtcNow;
            subTopicMaster.CreatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(subTopicMaster), Encoding.UTF8, "application/json");
            var result = await _httpHandler.PostAsync(subtopicsUri, stringContent);
            var responseContent = await result.Content.ReadAsStringAsync();
            SubTopicMaster subTopic = JsonConvert.DeserializeObject<SubTopicMaster>(responseContent);
            return Json(new { id = subTopic.SubTopicId, value = subTopic.SubTopic });
        }
       
        public async Task<bool> Update(SubTopicMaster subTopicMaster)
        {
            var uri = $"{subtopicsUri}/{subTopicMaster.SubTopicId}";
            var data = await _httpHandler.GetAsync<SubTopicMaster>(uri);
            data.SubTopic = subTopicMaster.SubTopic;
            data.UpdatedDttm = DateTime.UtcNow;
            data.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(uri, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id)
        {
            SubTopicMaster subTopicMaster = new();
            subTopicMaster = await _httpHandler.GetAsync<SubTopicMaster>(subtopicsUri + "/" + id);
            subTopicMaster.Status = false;
            subTopicMaster.UpdatedDttm = DateTime.UtcNow; 
            subTopicMaster.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(subTopicMaster), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(subtopicsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Restore(int id)
        {
            SubTopicMaster subTopicMaster = new();
            subTopicMaster = await _httpHandler.GetAsync<SubTopicMaster>(subtopicsUri + "/" + id);
            subTopicMaster.Status = true;
            subTopicMaster.UpdatedDttm = DateTime.UtcNow;
            subTopicMaster.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(subTopicMaster), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(subtopicsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<IActionResult> GetById(int id)
        {
            SubTopicMaster subTopic = new SubTopicMaster();
            subTopic = await _httpHandler.GetAsync<SubTopicMaster>(subtopicsUri + "/" + id);
            var result = await GetAllById(subTopic.TopicId);
            return result;
        }
        //public async Task<SubTopicMaster> GetTopicbySubtopic(int id)
        //{
        //    SubTopicMaster subtopic = new();
        //    try
        //    {
        //        string uri = $"{subtopicsUri}/GetTopicbySubtopic/{id}";
        //        subtopic = await _httpHandler.GetAsync<SubTopicMaster>(uri);
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex.Message.ToString());
        //    }

        //    return subtopic;
        //}
    }
}
