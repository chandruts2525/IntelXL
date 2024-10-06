using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorCertificationsController : GenericController<TutorCertification>
    {
        private readonly IntelxlContext _context;
        public TutorCertificationsController(IntelxlContext context) : base(context)
        {
            _context = context;
        }

    }
}
