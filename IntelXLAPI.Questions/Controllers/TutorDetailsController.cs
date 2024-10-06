using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorDetailsController : GenericController<TutorDetail>
    {
        private readonly ILogger<TutorDetailsController> _logger;
        private readonly IntelxlContext _context;

        public TutorDetailsController(ILogger<TutorDetailsController> logger, IntelxlContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }
    }
}
