using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class UnitsController : Controller
    {
        private readonly IHttpHandler _httpHandler;
        private readonly string? unitsUri;
        private readonly string? subjectUri; 
        private int _userId;
        private readonly string? baseUri;
        public UnitsController(IHttpHandler httpHandler, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            unitsUri = baseUri + IntelXlApiEnum.Units;
            subjectUri = baseUri + IntelXlApiEnum.Subjects; 
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
            SubjectMaster subject = new();
            string uri = $"{subjectUri}/GetListById/{id}";
            subject = await _httpHandler.GetAsync<SubjectMaster>(uri);
            return Json(subject.UnitMasters);
        }
        public async Task<IActionResult> AddUnit(UnitMaster unit)
        {
            unit.CreatedDttm = DateTime.UtcNow;
            unit.CreatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(unit), Encoding.UTF8, "application/json");
            var result = await _httpHandler.PostAsync(unitsUri, stringContent);
            var responseContent = await result.Content.ReadAsStringAsync();
            UnitMaster unitMaster = JsonConvert.DeserializeObject<UnitMaster>(responseContent);
            return Json(new { id = unitMaster.UnitId, value = unitMaster.UnitName });
        }
        
        public async Task<bool> Update(UnitMaster unit)
        {
            var uri = $"{unitsUri}/{unit.UnitId}";
            var data = await _httpHandler.GetAsync<UnitMaster>(uri);
            data.UnitName = unit.UnitName;
            data.UpdatedDttm= DateTime.UtcNow;
            data.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(uri, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id)
        {
            UnitMaster unit = new();
             unit = await _httpHandler.GetAsync<UnitMaster> (unitsUri + "/" + id);
             unit.Status = false;
            unit.UpdatedDttm=DateTime.UtcNow;
            unit.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(unit), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(unitsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> Restore(int id)
        {
            UnitMaster unit = new();
            unit = await _httpHandler.GetAsync<UnitMaster>(unitsUri + "/" + id);
            unit.Status = true;
            unit.UpdatedDttm = DateTime.UtcNow;
            unit.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(unit), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(unitsUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
