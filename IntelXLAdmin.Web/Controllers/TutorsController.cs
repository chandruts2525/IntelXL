using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class TutorsController : Controller
    {
        private readonly ILogger<TutorsController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string tutorsUri;
        private readonly string? baseUri;
        public TutorsController(ILogger<TutorsController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            _logger = logger;
            tutorsUri = baseUri + IntelXlApiEnum.Tutors;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> tutors = await GetTutors();
            return View(tutors);
        }
        public async Task<List<AppUser>> GetTutors()
        {
            List<AppUser> tutors = new();
            try
            {
                tutors = await _httpHandler.GetAsync<List<AppUser>>(tutorsUri + "/GetAllUnverifiedTutors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return tutors;
        }
        public async Task<IActionResult> Details(int tutorId)
        {
            AppUser tutor = new();
            try
            {
                tutor = await _httpHandler.GetAsync<AppUser>(tutorsUri + "/GetTutorById/" + tutorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(tutor);
        }
        public async Task<IActionResult> Verify(int tutorId)
        {
            bool result = false;
            string message = "";
            try
            {
                AppUser tutor = new();
                string endpoint = $"{tutorsUri}/{tutorId}";
                tutor = await _httpHandler.GetAsync<AppUser>(endpoint);
                if (tutor != null)
                {
                    tutor.IsVerified = true;
                    var stringContent = new StringContent(JsonConvert.SerializeObject(tutor), Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(tutorsUri + "/" + tutor.AppUserId, stringContent);
                    result = httpResponseMessage.IsSuccessStatusCode;
                }
                else
                {
                    message = "You should not verify without details";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
                message = "Something went wrong";
            }
            return Json(new { result, message });
        }
    }
}
