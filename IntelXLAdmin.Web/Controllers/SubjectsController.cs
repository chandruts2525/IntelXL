using IntelXL.HttpHandler;
using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Configuration;
using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly IHttpHandler _httpHandler;
        private readonly ILogger<SubjectsController> _logger;
        private readonly string? subjectUri;
        private readonly string? classUri; 
        private int _userId;
        private readonly string? baseUri;
        public SubjectsController(ILogger<SubjectsController> logger,IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            subjectUri = baseUri + IntelXlApiEnum.Subjects;
            classUri = baseUri + IntelXlApiEnum.Classes;
            _logger = logger; ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
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
            ClassMaster classes = new();
            string uri = $"{classUri}/GetListById/{id}";
            classes = await _httpHandler.GetAsync<ClassMaster>(uri);
            return Json(classes.SubjectMasters);
        }
        public async Task<IActionResult> AddSubject(SubjectMaster subject)
        {
            SubjectMaster subjectMaster = new();
            try
            {
                subject.CreatedDttm = DateTime.UtcNow;
                subject.CreatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
                var result = await _httpHandler.PostAsync(subjectUri, stringContent);
                var responseContent = await result.Content.ReadAsStringAsync();
                subjectMaster = JsonConvert.DeserializeObject<SubjectMaster>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }           
            return Json(new { id = subjectMaster?.SubjectId, value = subjectMaster?.SubjectName });            
        }
       
        public async Task<bool> Update(SubjectMaster subject)
        {
            var uri = $"{subjectUri}/{subject.SubjectId}";
            var data = await _httpHandler.GetAsync<SubjectMaster>(uri);
            data.SubjectName = subject.SubjectName;
            data.UpdatedDttm = DateTime.UtcNow;
            data.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(uri, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id)
        {
            SubjectMaster subject = new();
            subject = await _httpHandler.GetAsync<SubjectMaster>(subjectUri + "/" + id);
            subject.Status = false;
            subject.UpdatedDttm = DateTime.UtcNow;
            subject.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(subjectUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Restore(int id)
        {
            SubjectMaster subject = new();
            subject = await _httpHandler.GetAsync<SubjectMaster>(subjectUri + "/" + id);
            subject.Status = true;
            subject.UpdatedDttm = DateTime.UtcNow;
            var stringContent = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(subjectUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
