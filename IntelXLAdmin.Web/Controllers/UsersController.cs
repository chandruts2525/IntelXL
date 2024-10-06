using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IntelXLWeb.Models;

namespace IntelXLAdmin.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string userUri;      
        private readonly string roleUri;
        private int _userId;
        private readonly string? baseUri;
        public UsersController(ILogger<UsersController> logger, IHttpHandler httpHandler,IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            userUri = baseUri + IntelXlApiEnum.User;           
            _logger = logger;
            roleUri= baseUri + IntelXlApiEnum.Roles;
            ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _userId = userId;
            }
        }
        public IActionResult Index(int page = 1)
        {
            //List<AppUser> users = new List<AppUser>();  
            try
            {
                ViewBag.CurrentPage = page;
                //string endpoint = $"{userUri}/GetAllUsers/{page}";
                //using (var response = await _httpHandler.GetAsync(endpoint))
                //{
                //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        var responseData = await response.Content.ReadAsStringAsync();
                //        if (responseData != null)
                //        {
                //            var content = JsonConvert.DeserializeObject<PagedUsers>(responseData);
                //            ViewBag.TotalPages = content?.TotalPages;
                //            users = content?.Users;
                //        }
                //    }
                //}               
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }           
            return View(/*users*/);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        public async Task<IActionResult> SignIn(string emailId, string password, bool isPersist = false,string returnUrl="/Home/Index")
        {
            AppUser user = await ValidateUser(emailId, password);
            try
            {
                if (!string.IsNullOrEmpty(user.EmailId) && !string.IsNullOrEmpty(user.FirstName))
                {
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.EmailId),                   
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("UserID", user.AppUserId.ToString()),

                }; 
                    if (!string.IsNullOrEmpty(user?.AppRole?.RoleName))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, user.AppRole.RoleName));                      
                    }                     

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = isPersist,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),                        
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                    //return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<AppUser> ValidateUser(string userId, string password)
        {
            AppUser result = new AppUser();
            password = EnctyptPassword(password);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = userUri + "/Login?userid=" + userId + "&password=" + password;

                    using (HttpResponseMessage response = await client.GetAsync(endpoint))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<AppUser>(responseContent);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var roles = await GetRoles();
            ViewBag.Roles = new SelectList(roles, "AppRoleId", "RoleName");
            return View();
        }
        public async Task<List<AppRole>> GetRoles()
        {
            List<AppRole> roles=new List<AppRole>();
            try
            {
                roles = await _httpHandler.GetAsync<List<AppRole>>(roleUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }           
            return roles;
        }
        [HttpPost]
        public async Task<bool> Registration(AppUser user)
        {
            bool result = false;
            try
            {
                user.UserName = user.EmailId.Split('@')[0];
                if (string.IsNullOrEmpty(user.LastName))
                {
                    user.LastName = "IntelXl";
                }
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = EnctyptPassword(user.Password);
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((userUri), stringContent);
                result = apiResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return result;
        }
        public string EnctyptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public async Task<bool> IsUserExist(string emailId)
        {
            bool result = false;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = userUri + "/IsUserExist/" + emailId;

                using (HttpResponseMessage response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public async Task<bool> UpdateUserStatus(int id,bool status)
        {
            bool result = false;
            try
            {
                AppUser appUser = new();
                appUser = await _httpHandler.GetAsync<AppUser>(userUri + "/" + id);
                appUser.Status = status;
                appUser.UpdatedDttm = DateTime.UtcNow; ;
                appUser.UpdatedBy = _userId;
                var stringContent = new StringContent(JsonConvert.SerializeObject(appUser), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(userUri + "/" + id, stringContent);               
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return result;
        }

        public async Task<IActionResult> Details(int id)
        {
            AppUser appUser = new();
            try
            {
                string endpoint = $"{userUri}/GetUserById/{id}";
                appUser = await _httpHandler.GetAsync<AppUser>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(appUser);
        }

        public IActionResult SearchUser(string searchTerm, int page = 1)
        {
            return ViewComponent("Users", new { page = page, searchTerm = searchTerm});
            
        }
    }
}
