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
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IHttpHandler _httpHandler;    
        private readonly string? courseUri; 
        private readonly string languageOfInstructionsUri;
        private int _userId;
        private readonly string? baseUri;

        public CourseController(ILogger<CourseController> logger,IHttpHandler httpHandler,IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;           
            courseUri = baseUri + IntelXlApiEnum.Courses;
            _logger = logger; 
            languageOfInstructionsUri = baseUri + IntelXlApiEnum.LanguageOfInstructions;
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
        
        public async Task<IActionResult> AddCourse(CourseMaster course)
        {
            CourseMaster courseMaster = new();
            try
            {
                course.CreatedDttm = DateTime.UtcNow;
                course.CreatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
                var result = await _httpHandler.PostAsync(courseUri, stringContent);
                var responseContent = await result.Content.ReadAsStringAsync();
                courseMaster = JsonConvert.DeserializeObject<CourseMaster>(responseContent);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message.ToString());
            }
                    
            return Json(new { id = courseMaster.CourseId, value = courseMaster.CourseName });
        }
        
        public async Task<IActionResult> EditOrDelete(int id)
        {
            List<CourseMaster> courseList= await _httpHandler.GetAsync<List<CourseMaster>>(courseUri + "/GetAllCourse/" + id);
            return Json(courseList);
        }
        
        public async Task<bool> Update(CourseMaster course)
        {
            bool result = false;
            try
            {
                var uri = $"{courseUri}/{course.CourseId}";
                var data = await _httpHandler.GetAsync<CourseMaster>(uri);
                data.CourseName = course.CourseName;
                data.UpdatedDttm= DateTime.UtcNow;
                data.UpdatedBy= _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(uri, stringContent);
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<bool> Delete(int id)
        {
            CourseMaster course = new();
            course = await _httpHandler.GetAsync<CourseMaster>(courseUri + "/" + id);
            course.Status = false;
            course.UpdatedDttm = DateTime.UtcNow;
            course.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(courseUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Restore(int id)
        {
            CourseMaster course = new();
            course = await _httpHandler.GetAsync<CourseMaster>(courseUri + "/" + id);
            course.Status = true;
            course.UpdatedDttm = DateTime.UtcNow;
            course.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(courseUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<IActionResult> GetAllById(int id)
        {
            LanguageOfInstructionMaster language = new();
            string uri = $"{languageOfInstructionsUri}/GetListById/{id}";
            language = await _httpHandler.GetAsync<LanguageOfInstructionMaster>(uri);
            return Json(language.CourseMasters);
        }
    }
}
