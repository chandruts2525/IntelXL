using IntelXLAdmin.Api.Controllers;

using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginsController : GenericController<UserLogin>
    {
        private readonly IntelxlContext _context;
        public UserLoginsController(IntelxlContext context) : base(context)
        {
            _context = context;
        }
    }
}
