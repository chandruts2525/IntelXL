using IntelXL.HttpHandler;
using IntelXLDataAccess.Models;
using IntelXLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace IntelXLWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string? baseUri;
        private readonly string _courseUri;
        public readonly string _classesUri;
        public readonly IHttpHandler _httpHandler;
        private readonly IConfiguration _configuration;
        public readonly int languageId;
        public int courseId;
        private readonly string languageOfInstructionsUri;
        public int multiplyBy;
        public HomeController(ILogger<HomeController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            baseUri = configuration.GetValue<string>("baseUrl");
            _classesUri = baseUri + IntelXlApiEnum.Classes;
            _courseUri = baseUri + IntelXlApiEnum.Courses;
            _httpHandler = httpHandler;
            languageOfInstructionsUri = baseUri + IntelXlApiEnum.LanguageOfInstructions;
            languageId = _configuration.GetValue<int>("LanguageId");
            courseId = _configuration.GetValue<int>("CourseId");
            multiplyBy = _configuration.GetValue<int>("MultiplyBy");
        }

        public async Task<IActionResult> Index(bool isSignedIn = false)
        {
            try
            {
                //Remove after mobile subscription
                ViewBag.IsSignedIn = isSignedIn;

                if (User.Identity.IsAuthenticated)
                {
                    int userId = 0;
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim tutorIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (tutorIdClaim != null)
                    {
                        userId = int.Parse(tutorIdClaim.Value);
                    }
                    if (User.IsInRole("Tutor"))
                    {
                        return RedirectToAction("Profile", "User", new { userId = userId });
                    }
                }
                List<LanguageOfInstructionMaster> languages = await _httpHandler.GetAsync<List<LanguageOfInstructionMaster>>(languageOfInstructionsUri);
                languages = languages.Where(lang => lang.Status).OrderBy(l => l.Order).ToList();
                ViewBag.languages = new SelectList(languages, "LanguageId", "Language");
                var courses = await GetCourses(languages[0].LanguageId);
                ViewBag.Courses = courses;
                if (courses[0].CourseId != 0)
                    courseId = courses[0].CourseId;
                ViewBag.CourseId = courseId;
                ViewBag.MultiplyBy = multiplyBy;
                //ViewBag.Courses = await _httpHandler.GetAsync<List<CourseMaster>>(_courseUri + "/GetAllCourse/"+ languageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return View();
        }
        public IActionResult SessionExpired()
        {
            return View();
        }
        public IActionResult UnderDevelopment()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<List<CourseMaster>> GetCourses(int id)
        {
            var courses = await _httpHandler.GetAsync<List<CourseMaster>>(_courseUri + "/GetAllCourse/" + id);
            courses = courses.OrderBy(c => c.Order).ToList();
            return courses;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<ActionResult> GetSliderAsync()
        {
            List<CourseMaster> courses = new();
            try
            {
                courses = await _httpHandler.GetAsync<List<CourseMaster>>(_courseUri + "/GetAllIncludeSubject");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return PartialView("_LearningSlide", courses);
        }
        public IActionResult Awards()
        {
            return View();
        }
        public IActionResult Company()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Careers()
        {
            return View();
        }
    }
}