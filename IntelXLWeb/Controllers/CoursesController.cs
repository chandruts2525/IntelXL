using Microsoft.AspNetCore.Mvc;

namespace IntelXLWeb.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
