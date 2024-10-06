using Microsoft.AspNetCore.Mvc;

namespace IntelXLWeb.Controllers
{
    public class SubjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
