using IntelXLDataAccess.Data.Repo;
using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;

namespace IntelXLAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : GenericController<AppRole>
    {
        private readonly IntelxlContext _context;
        private readonly IBaseRepo<AppRole> _baseRepo;
        public RolesController(IBaseRepo<AppRole> baseRepo, IntelxlContext context) : base(context)
        {
            _baseRepo = baseRepo;
            _context = context;
        }
    }
}
