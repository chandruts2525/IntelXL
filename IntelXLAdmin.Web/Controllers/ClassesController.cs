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
    public class ClassesController : Controller
    {
        private readonly IHttpHandler _httpHandler;
        private readonly ILogger<ClassesController> _logger;

        private readonly string? classUri;
        private readonly string? courseUri;
        private int _userId;
        private readonly string? baseUri;
        public ClassesController(ILogger<ClassesController> logger, IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            classUri = baseUri + IntelXlApiEnum.Classes;
            courseUri = baseUri + IntelXlApiEnum.Courses;
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
        public async Task<IActionResult> AddClass(ClassMaster classes)
        {
            ClassMaster classMaster = new ClassMaster();
            try
            {
                classes.CreatedDttm = DateTime.UtcNow;
                classes.CreatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(classes), Encoding.UTF8, "application/json");
                var result = await _httpHandler.PostAsync(classUri, stringContent);
                var responseContent = await result.Content.ReadAsStringAsync();
                classMaster = JsonConvert.DeserializeObject<ClassMaster>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return Json(new { id = classMaster?.ClassId, value = classMaster?.ClassName });

        }

        public async Task<IActionResult> GetAllById(int id)
        {
            CourseMaster course = new();
            string uri = $"{courseUri}/GetListById/{id}";
            course = await _httpHandler.GetAsync<CourseMaster>(uri);
            return Json(course.ClassMasters);
        }
        public async Task<IActionResult> EditOrDelete()
        {
            List<ClassMaster> classList=new List<ClassMaster>();
            try
            {
                classList = await _httpHandler.GetAsync<List<ClassMaster>>(classUri);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
           
            return Json(classList);
        }
        public async Task<bool> Update(ClassMaster classes)
        {
            bool result = false;
            try
            {
                var uri = $"{classUri}/{classes.ClassId}";
                var data = await _httpHandler.GetAsync<ClassMaster>(uri);
                data.ClassName = classes.ClassName;
                data.Description= classes.Description;
                data.UpdatedDttm= DateTime.UtcNow;
                data.UpdatedBy = _userId;
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
            ClassMaster classes = new();
            bool response = false;
            try
            {
                classes = await _httpHandler.GetAsync<ClassMaster>(classUri + "/" + id);
                classes.Status = false;
                classes.UpdatedDttm= DateTime.UtcNow;
                classes.UpdatedBy= _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(classes), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(classUri + "/" + id, stringContent);
                response = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
        public async Task<bool> Restore(int id)
        {
            ClassMaster classes = new();
            classes = await _httpHandler.GetAsync<ClassMaster>(classUri + "/" + id);
            classes.Status = true;
            classes.UpdatedDttm = DateTime.UtcNow;
            classes.UpdatedBy = _userId; 
            var stringContent = new StringContent(JsonConvert.SerializeObject(classes), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(classUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
