using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Drawing.Printing;
using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class LanguageOfInstructionsController : Controller
    {
        private readonly ILogger<LanguageOfInstructionsController> _logger;
        private readonly IHttpHandler _httpHandler;        
        private readonly string languageOfInstructionsUri;
        private int _userId;
        private readonly string? baseUri;
        public LanguageOfInstructionsController(ILogger<LanguageOfInstructionsController> logger, IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
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
        public async Task<IActionResult> AddLanguage(LanguageOfInstructionMaster lang)
        {
            LanguageOfInstructionMaster languageMaster = new();
            try
            {
                lang.CreatedDttm = DateTime.UtcNow;
                lang.CreatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(lang), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((languageOfInstructionsUri), stringContent);
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                languageMaster = JsonConvert.DeserializeObject<LanguageOfInstructionMaster>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Json(new { id = languageMaster.LanguageId, value = languageMaster.Language });
        }
        public async Task<bool> Update(LanguageOfInstructionMaster lang)
        {
            lang.UpdatedDttm = DateTime.UtcNow;
            lang.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(lang), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(languageOfInstructionsUri + "/" + lang.LanguageId, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id)
        {
            bool result = false;
            LanguageOfInstructionMaster lang = new();
            try
            {
                lang = await _httpHandler.GetAsync<LanguageOfInstructionMaster>(languageOfInstructionsUri + "/" + id);
                lang.Status = false; 
                lang.UpdatedDttm = DateTime.UtcNow;
                lang.UpdatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(lang), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(languageOfInstructionsUri + "/" + id, stringContent);
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<bool> Restore(int id)
        {
            bool result = false;
            LanguageOfInstructionMaster lang = new();
            try
            {
                lang = await _httpHandler.GetAsync<LanguageOfInstructionMaster>(languageOfInstructionsUri + "/" + id);
                lang.Status = true;
                lang.UpdatedDttm = DateTime.UtcNow;
                lang.UpdatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(lang), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(languageOfInstructionsUri + "/" + id, stringContent);
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
    }
}
