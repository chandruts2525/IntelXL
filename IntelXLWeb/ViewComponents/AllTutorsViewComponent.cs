using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IntelXLWeb.ViewComponents
{
    public class AllTutorsViewComponent : ViewComponent
    {
        private readonly ILogger<AllTutorsViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string tutorsUri; private readonly string? baseUri;
        public AllTutorsViewComponent(ILogger<AllTutorsViewComponent> logger, IHttpHandler httpHandler,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            tutorsUri = baseUri + IntelXlApiEnum.Tutors;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<AppUser> response = new List<AppUser>();
            try
            {
                response = await _httpHandler.GetAsync<List<AppUser>>(tutorsUri + "/GetVerifiedTutors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(response);
        }
    }
}
