using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : GenericController<AnswerMaster>
    {
        private readonly IntelxlContext _context;

        public AnswersController(IntelxlContext context) : base(context)
        {
            _context = context;
        }
    }
}
