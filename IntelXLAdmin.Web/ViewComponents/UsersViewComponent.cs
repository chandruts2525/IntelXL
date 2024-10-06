using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Drawing.Printing;

namespace IntelXLAdmin.Web.ViewComponents
{
    public class UsersViewComponent : ViewComponent
    {
        private readonly ILogger<UsersViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;       
        private readonly string? userUri;
        private readonly string? baseUri;
        public UsersViewComponent(ILogger<UsersViewComponent> logger, IHttpHandler httpHandler,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;             
            userUri = baseUri + IntelXlApiEnum.User;           
        }
        public async Task<IViewComponentResult> InvokeAsync(int page,string searchTerm="")
        {
            List<AppUser> users = new List<AppUser>(); 
          
            try
            {
                ViewBag.CurrentPage = page;
                string endpoint = $"{userUri}/GetAllUsers/{page}";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    endpoint = $"{userUri}/SearchUser/{page}?searchTerm={searchTerm}";
                    ViewBag.SearchTerm=searchTerm;
                }
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            var content = JsonConvert.DeserializeObject<PagedUsers>(responseData);
                            ViewBag.TotalPages = content?.TotalPages;
                            ViewBag.ItemsPerPage = content?.PageSize;
                            users = content?.Users;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }            
            return View(users);
        }

    }
}
