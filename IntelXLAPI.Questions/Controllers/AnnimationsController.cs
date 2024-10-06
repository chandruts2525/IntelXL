using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnimationsController : GenericController<AnnimationMaster>
    {
        private readonly IntelxlContext _context;

        public AnnimationsController(IntelxlContext context) : base(context)
        {
            _context = context;
        }
    }
}
