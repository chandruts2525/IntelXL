using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorSchedulesController : GenericController<StudentTutorSchedule>
    {
        private readonly ILogger<TutorSchedulesController> _logger;
        private readonly IntelxlContext _context;

        public TutorSchedulesController(ILogger<TutorSchedulesController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }

    }
}
