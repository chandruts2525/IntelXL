using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimingsController : GenericController<TimingMaster>
    {
        private readonly IntelxlContext _context;
        public TimingsController(IntelxlContext context) : base(context)
        {
            _context = context;
        }

    }
}
