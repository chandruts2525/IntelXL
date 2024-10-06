using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string _paymentsUri;
        private readonly string? baseUri;

        public PaymentsController(ILogger<PaymentsController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;          
            _logger = logger;            
            _paymentsUri = baseUri + IntelXlApiEnum.Payments;
            //pageSize = _configuration.GetValue<int>("PageSize");
        }
        public async Task<IActionResult> Index(int page=1)
        {
            List<UserPayment> payments = new List<UserPayment>();
            try
            {
                ViewBag.CurrentPage = page;
                //ViewBag.ItemsPerPage = pageSize;

                string endpoint = $"{_paymentsUri}/GetAllPayments/{page}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            var content = JsonConvert.DeserializeObject<PagedPayments>(responseData);
                            ViewBag.TotalPages = content?.TotalPages;
                            payments = content?.Payments;
                        }
                    }
                }
                //string endpoint = _paymentsUri + "/GetAllPayments/pageNum={page}";
                //payments = await _httpHandler.GetAsync<List<UserPayment>>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }           
            return View(payments);
        } 
        
        public async Task<IActionResult> Details(int id)
        {
            UserPayment userPayment = new();
            try
            {
                string endpoint = $"{_paymentsUri}/GetDetailsById/{id}";
                userPayment = await _httpHandler.GetAsync<UserPayment>(endpoint);               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }           
            return View(userPayment);
        }
    }
}
