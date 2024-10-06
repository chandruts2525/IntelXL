using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IntelXLWeb.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IHttpHandler _handler;
        private readonly string _classUrl;
        private readonly ILogger<ClassesController> _logger;
        private readonly string? baseUri;

        public ClassesController(ILogger<ClassesController> logger, IHttpHandler handler, IConfiguration configuration)
        {
            _handler = handler;
            _logger = logger; 
            baseUri = configuration.GetValue<string>("baseUrl");
            _classUrl = baseUri+ IntelXlApiEnum.Classes; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IEnumerable<ClassMaster>> GetAll()
        {
            IEnumerable<ClassMaster> classes = new List<ClassMaster>();
            try
            {
                classes = await _handler.GetAsync<IEnumerable<ClassMaster>>(_classUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return classes;
        }
    }
}
