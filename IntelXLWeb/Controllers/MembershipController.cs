using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntelXLWeb.Controllers
{
    public class MembershipController : Controller
    {
        private readonly string? baseUri;
        private readonly IHttpHandler _httpHandler;
        private readonly string courseUri;
        public MembershipController(IHttpHandler httpHandler, IConfiguration configuration)
        {
            _httpHandler = httpHandler; 
            baseUri = configuration.GetValue<string>("baseUrl");
            courseUri = baseUri + IntelXlApiEnum.Courses;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeachersTrial()
        {
            return View();
        }
        public IActionResult Trial()
        {
            return View();
        }
        public IActionResult quote()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult Policy()
        {
            return View();
        }
        public async Task<IActionResult> Join(bool isSignedIn = false)
        {
            //Remove after mobile subscription
            ViewBag.IsSignedIn = isSignedIn;

            List<CourseMaster> courses = await _httpHandler.GetAsync<List<CourseMaster>>(courseUri);
            courses = courses.Where(c => c.Status).ToList();
            ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");
            return View();
        }
        public IActionResult inspriation()
        {
            return View();
        }
        public async Task<List<CourseMaster>> GetCourses()
        {
            var courses = await _httpHandler.GetAsync<List<CourseMaster>>(courseUri + "/GetAllCourse");
            return courses;
        }
        public IActionResult GetSubscriptions(int courseId,int classId)
        {
            return ViewComponent("AllSubscriptions", new { courseId = courseId, classId = classId });
        }
        public async Task<IActionResult> GetAllById(int id)
        {
            CourseMaster course = new();
            string uri = $"{courseUri}/GetListById/{id}";
            course = await _httpHandler.GetAsync<CourseMaster>(uri);
            return Json(course.ClassMasters);
        }
    }
}
