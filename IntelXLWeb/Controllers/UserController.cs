using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IntelXLWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string userUri;
        private readonly string userLoginUri;
        private readonly string? baseUri;
        public UserController(ILogger<UserController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            _logger = logger;
            _httpHandler = httpHandler;
            baseUri = configuration.GetValue<string>("baseUrl");
            userUri = baseUri + IntelXlApiEnum.User;
            userLoginUri = baseUri + IntelXlApiEnum.UserLogins;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUser user)
        {
            try
            {
                user.UserName = user.EmailId.Split('@')[0];
                //user.AppRoleId = 1;
                user.CreatedDttm = DateTime.UtcNow;                
                if (string.IsNullOrEmpty(user.LastName))
                {
                    user.LastName = "";
                }
                var password = user.Password;
                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = await EnctyptPassword(password);
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((userUri), stringContent);                
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                AppUser? appUser = JsonConvert.DeserializeObject<AppUser>(responseContent);
                if (apiResponse.IsSuccessStatusCode)
                {
                    if (appUser != null)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, appUser.EmailId),
                            new Claim("UserID", appUser.AppUserId.ToString()),
                            new Claim(ClaimTypes.Name, appUser.FirstName),
                            new Claim(ClaimTypes.Role, "user")
                        };
                    }
                    return RedirectToAction("Signin", "User", new { emailId = user.EmailId, password = password });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<string> EnctyptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Signin(string emailId, string password, string returnUrl = "/Home", bool isPersist = false)
        {
            AppUser user = await ValidateUser(emailId, password);
            try
            {
                if (!string.IsNullOrEmpty(user.EmailId) && !string.IsNullOrEmpty(user.FirstName))
                {
                    var subscriptionData = user.UserSubscriptions.Where(u => u.Status == true).ToList();
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.EmailId),
                    new Claim("UserID", user.AppUserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.AppRole.RoleName)
                };
                    claims.Add(new Claim("IPAddress", GetIpAddress()));
                    List<int> classId = new List<int>();
                    if (subscriptionData != null)
                    {
                        foreach (var data in subscriptionData)
                        {
                            if (data != null && data.Subscription != null && data.Subscription.ClassId!=0)
                            {
                                classId.Add(data.Subscription.ClassId);
                            }
                        }
                    }
                    string classIdString = string.Join(",", classId);

                    //DateTime expirationTime = DateTime.Now.AddMinutes(60);
                    //CookieOptions cookieOptions = new CookieOptions
                    //{
                    //    Expires = expirationTime,
                    //    HttpOnly = true,
                    //    Secure = true
                    //};
                    //this.Response.Cookies.Append("ixl_c", classIdString, cookieOptions);
                    claims.Add(new Claim("ClassId", classIdString));
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = isPersist,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    };
                    await UserTracking(user, true);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                    if (user.AppRole.RoleName == "Tutor")
                    {
                        returnUrl = $"/User/Profile?userId={user.AppUserId}";
                    }                   
                    return Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Redirect(returnUrl);
        }
        public async Task<IActionResult> LogOut()
        {
            Response.Cookies.Delete("ixl_c");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<AppUser> ValidateUser(string userId, string password)
        {
            AppUser result = new AppUser();
            password = await EnctyptPassword(password);
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


        public async Task GoogleLogin()
        {
            var properties = new AuthenticationProperties()
            { RedirectUri = Url.Action("GoogleResponse", "User") };

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var claims = new List<Claim>(result.Principal.Claims);

                claims.Add(new Claim(ClaimTypes.Role, "User"));

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                AppUser user = new AppUser
                {
                    FirstName = result.Principal.FindFirst(ClaimTypes.Name).Value,
                    EmailId = result.Principal.FindFirst(ClaimTypes.Email).Value,
                };
                bool exists = await IsUserExist(user.EmailId);
                if (exists)
                {
                    string endpoint = userUri + "/IsUserExist/" + user.EmailId;
                    AppUser userdata = await _httpHandler.GetAsync<AppUser>(endpoint);
                    if (!string.IsNullOrEmpty(userdata.EmailId) && !string.IsNullOrEmpty(userdata.UserName))
                    {
                        claims.Add(new Claim("UserID", userdata.AppUserId.ToString()));
                        //claims.Add(new Claim(ClaimTypes.Role, userdata.AppRole.RoleName));
                        var newIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var newprincipal = new ClaimsPrincipal(newIdentity);
                        await UserTracking(userdata, true);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newprincipal);
                    }
                }
                else
                {
                    user.AppRoleId = 1;
                    await Register(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile(int userId)
        {
            AppUser user = new();
            try
            {
                string endpoint = $"{userUri}/GetUserById/{userId}";
                user = await _httpHandler.GetAsync<AppUser>(endpoint);
                if(user!=null)
                {
                    if (user.AppRoleId == 4)
                    {
                        return View("TutorProfile", user);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(user);
        }
        public async Task<bool> UserTracking(AppUser user, bool IsLogin)
        {
            bool result = false;
            HttpResponseMessage httpResponseMessage;
            try
            {
                UserLogin userLogin = new UserLogin
                {
                    UserId = user.AppUserId,
                    DeviceIpAddress = GetIpAddress(),
                    LoginDttm = DateTime.Now,
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");
                if (user.UserLogin != null)
                {
                    httpResponseMessage = await _httpHandler.PutAsync(userLoginUri + "/" + userLogin.UserLoginId, stringContent);
                }
                else
                {
                    httpResponseMessage = await _httpHandler.PostAsync(userLoginUri, stringContent);
                }
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return result;
        }
        public string GetIpAddress()
        {
            string ipAddress = string.Empty;

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return ipAddress;
        }
        public async Task<IActionResult> GetuserDetail(int userId)
        {
            AppUser user = new();
            try
            {
                string endpoint = $"{userUri}/GetUserById/{userId}";
                user = await _httpHandler.GetAsync<AppUser>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Json(user);
        }
        public async Task<bool> UpdateUser(AppUser appuser)
        {
            bool result=false;
            try
            {
                appuser.UserName = appuser.EmailId.Split('@')[0];
                appuser.UpdatedDttm = DateTime.Now;
                appuser.IsVerified = true;
                var stringContent = new StringContent(JsonConvert.SerializeObject(appuser), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(userUri + "/" + appuser.AppUserId, stringContent);
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return result;
        }
        public async Task<IActionResult> ChangePassword(int userId,string currentPwd,string newPwd)
        {
            bool result = false;
            string message = "";
            AppUser user = new();
            try
            {
                string endpoint = $"{userUri}/GetUserById/{userId}";
                user = await _httpHandler.GetAsync<AppUser>(endpoint);
                if(user != null)
                {
                    if(user.Password!=await EnctyptPassword(currentPwd))
                    {
                        message = "Current password is wrong";
                        result = false;
                    }
                    else
                    {
                        user.Password=await EnctyptPassword(newPwd);
                        var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                        HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(userUri + "/" + user.AppUserId, stringContent);
                        result = httpResponseMessage.IsSuccessStatusCode;                       
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Something went wrong. Try again later...";
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return Json(new { result, message });
        }
    }
}
