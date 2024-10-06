using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IntelXLAdmin.Web.ViewComponents
{
    public class QuestionsViewComponent : ViewComponent
    {
        private readonly ILogger<QuestionsViewComponent> _logger;
        private readonly IHttpHandler _httpHandler; 
        private readonly IConfiguration _configuration;
        private readonly int pageSize; 
        private readonly string? questionUri;
        private readonly string? baseUri;
        public QuestionsViewComponent(ILogger<QuestionsViewComponent> logger, IHttpHandler httpHandler,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler; 
            _configuration = configuration;
            questionUri = baseUri + IntelXlApiEnum.Questions;
            pageSize = _configuration.GetValue<int>("PageSize");
        }
        public async Task<IViewComponentResult> InvokeAsync(int id, int page, bool status=false)
        {
            List<QuestionMaster>? result = new List<QuestionMaster>();
            ViewBag.SubTopicId = id;
            ViewBag.CurrentPage = page;
            ViewBag.ItemsPerPage = pageSize;
            try
            {
                string endpoint = $"{questionUri}/GetQuestions/{id}?status={status}&pageNum={page}";
                //string endpoint = $"{questionUri}/GetQuestions/{id}?pageNum={page}&status={status}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            var content = JsonConvert.DeserializeObject<ArrangedQuestions>(responseData);
                            ViewBag.TotalPages = content?.TotalCount;
                            result = content?.Questions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }            
            return View(result);
        }

    }
}
