using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;
using IntelXLWeb.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Net;
using System.Text;

namespace IntelXLWeb.ViewComponents
{
    public class ClassesBysubjectViewComponent : ViewComponent
    {
        private readonly ILogger<ClassesBysubjectViewComponent> _logger;
        public readonly string? _coursesUri;
        public readonly IHttpHandler _httpHandler; 
        private readonly string? baseUri;

        public ClassesBysubjectViewComponent(ILogger<ClassesBysubjectViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _logger = logger;
            _coursesUri = baseUri + IntelXlApiEnum.Courses;
            _httpHandler = httpHandler;
        }
        public async Task<IViewComponentResult> InvokeAsync( int courseId,string subject,string viewName)
        {
            var viewModel = new SubjectBasedClassesViewModel();
            viewModel.SubTopics = new Dictionary<ClassMaster, List<SubTopicMaster>>();
           
            try
            {
                string decodedString = WebUtility.HtmlDecode(subject);

                byte[] bytes = Encoding.Unicode.GetBytes(decodedString);

                subject = Encoding.Unicode.GetString(bytes);
                viewModel.SubjectName = subject;
                var response = await _httpHandler.GetAsync<CourseMaster>($"{_coursesUri}/GetByIdIncludeAllAsync/{courseId}");
                if (response != null)
                {
                    viewModel.Classes = response.ClassMasters
                        .Where(cls => cls.SubjectMasters?.Any(sub => sub.SubjectName != null && sub.SubjectName.Contains(subject)) == true)
                        .OrderBy(c=>c.Order)
                        .ToList();
                 
                    viewModel.Subjects = viewModel.Classes
                        .SelectMany(cls => cls.SubjectMasters)
                        .DistinctBy(sub => sub.SubjectName)
                        .OrderBy(sub => sub.Order)
                        .ToList();

                    foreach (var item in viewModel.Classes)
                    {
                        if (item != null)
                        {
                            var subtopicList = item.SubjectMasters
                                .Where(sub => sub.SubjectName != null && sub.SubjectName.Contains(subject))
                                .SelectMany(sub => sub.UnitMasters)
                                .SelectMany(unit => unit.TopicMasters)
                                .SelectMany(topic => topic.SubTopicMasters)
                                .OrderBy(c => c.Order)
                                .ToList();

                            if (viewModel.SubTopics != null)
                            {
                                viewModel.SubTopics.Add(item, subtopicList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(viewModel);
        }

    }
}
