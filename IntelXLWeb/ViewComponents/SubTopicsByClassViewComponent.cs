using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;
using IntelXLWeb.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Net;
using System.Security.Claims;
using System.Text;

namespace IntelXLWeb.ViewComponents
{
    public class SubTopicsByClassViewComponent : ViewComponent
    {
        private readonly ILogger<SubTopicsByClassViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string? _coursesUri; 
        private readonly string? baseUri;
        private readonly string? _questionUri;
        private readonly List<int>? arrayOfClassId;
        private readonly int allowIndex;
        public SubTopicsByClassViewComponent(ILogger<SubTopicsByClassViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            arrayOfClassId= configuration.GetSection("ArrayOfClassId").Get<List<int>>();  
            _logger = logger;
            _httpHandler = httpHandler;
            _coursesUri = baseUri + IntelXlApiEnum.Courses; 
            _questionUri= baseUri + IntelXlApiEnum.Questions;
            allowIndex = configuration.GetValue<int>("AllowIndex");
        }
        public async Task<IViewComponentResult> InvokeAsync(int courseId, int classId, string subject)
        {
            string decodedString = WebUtility.HtmlDecode(subject);           
            byte[] bytes = Encoding.Unicode.GetBytes(decodedString);
            subject = Encoding.Unicode.GetString(bytes);
            var viewModel = new ClassBasedSubTopicsViewModel();          
            viewModel.SubjectName = subject;
            viewModel.ClassId = classId;
            viewModel.AllowIndex = allowIndex;
            int subjectId = 0;
            var classIdsStr = "";            
            try
            {
                var response = await _httpHandler.GetAsync<CourseMaster>($"{_coursesUri}/GetByIdIncludeAllAsync/{courseId}");
                if (arrayOfClassId != null && arrayOfClassId.Contains(classId))
                {
                    viewModel.IsEnabled = true;
                }
                if (response != null)
                {
                    viewModel.Topics = response.ClassMasters
                        .Where(x => x.ClassId == classId)
                        .SelectMany(cls => cls.SubjectMasters)
                        .Where(sub => sub != null && sub.SubjectName != null && sub.SubjectName.Contains(subject)).SelectMany(u => u.UnitMasters)
                        .SelectMany(subject => subject.TopicMasters)
                        .OrderBy(c => c.Order)
                        .ToList();
                    var subjects = response.ClassMasters
                        .Where(cls => cls.ClassId == classId)
                        .SelectMany(cls => cls.SubjectMasters.Where(sub => sub != null &&
                                                        sub.SubjectName != null &&
                                                        sub.SubjectName.Contains(subject)))
                        .FirstOrDefault();
                    subjectId = subjects?.SubjectId??0;

                    var classMaster = response.ClassMasters.FirstOrDefault(x => x.ClassId == classId);
                    if (classMaster != null)
                    {
                        viewModel.ClassName = classMaster.ClassName;
                        viewModel.Subjects= classMaster.SubjectMasters.OrderBy(c => c.Order).ToList();
                    }                    
                }
                if (User != null && User.Identity != null)
                {
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim classIdClaim = claims.FirstOrDefault(c => c.Type == "ClassId");
                    if (classIdClaim != null)
                    {
                        classIdsStr = classIdClaim.Value;
                    }
                }
                if (!string.IsNullOrEmpty(classIdsStr))
                {
                    var splitItems = classIdsStr.Split(',');
                    foreach (var item in splitItems)
                    {
                        if (int.TryParse(item, out int id))
                        {
                            viewModel.ClassIds.Add(id);
                        }
                    }
                    if (viewModel.ClassIds.Contains(classId))
                    {
                        viewModel.HasSubscription = true;
                    }
                }
                List<SelectListItem> selectListItems = new();
                if (viewModel.HasSubscription)
                {
                    List<string> years = await GetYears(subjectId);
                    selectListItems = years.OrderBy(year => year).Select(year => new SelectListItem
                    {
                        Value = year,
                        Text = year
                    }).ToList();
                }                
                ViewBag.Years = new SelectList(selectListItems, "Value", "Text");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return View(viewModel);
        }
        public async Task<List<string>> GetYears(int id)
        {
            List<string> years = new List<string>();
            try
            {
                //var response = await _httpHandler.GetAsync<List<string>>(_questionUri + "/GetAllYears");
                years = await _httpHandler.GetAsync<List<string>>($"{_questionUri}/GetAllYearsBySubject/{id}");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return years;
        }
    }
}
