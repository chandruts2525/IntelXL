using IntelXL.HttpHandler;
using IntelXLDataAccess.Models;
using IntelXLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Net;
using System.Text;

namespace IntelXLWeb.ViewComponents
{
    public class TopicsBySubjectViewComponent : ViewComponent
    {
        public readonly string? _coursesUri;
        public readonly IHttpHandler _httpHandler;
        private readonly ILogger<TopicsBySubjectViewComponent> _logger;
        private readonly string? baseUri;
        public TopicsBySubjectViewComponent(ILogger<TopicsBySubjectViewComponent> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            _coursesUri = baseUri + IntelXlApiEnum.Courses;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(int courseId, string subjectName)
        {
            var response = new List<SubTopicsThread>();

            Dictionary<string, SubTopicsThread> subTopicsDic = new Dictionary<string, SubTopicsThread>();
            

            try
            {
                string decodedString = WebUtility.HtmlDecode(subjectName);

                byte[] bytes = Encoding.Unicode.GetBytes(decodedString);

                subjectName = Encoding.Unicode.GetString(bytes);
                ViewBag.Subject = subjectName;
                ViewBag.CourseId = courseId;

                var resCourse = await _httpHandler.GetAsync<CourseMaster>($"{_coursesUri}/GetByIdIncludeAllAsync/{courseId}");

                ViewBag.Subjects = resCourse?.ClassMasters?
                            .SelectMany(classItem => classItem.SubjectMasters)
                            .OrderBy(s => s.Order)
                            .Select(subject => subject.SubjectName)
                            .Distinct()
                            .ToList();

                response = resCourse?.ClassMasters?
                                .SelectMany(classItem => classItem.SubjectMasters)
                                .Where(subject => subject.SubjectName == subjectName)
                                .SelectMany(subject => subject.UnitMasters)
                                .SelectMany(unit => unit.TopicMasters)
                                .GroupBy(topic => topic.Topic)
                                .Select(group => new SubTopicsThread
                                {
                                    Topic = group.Key,
                                    SubTopicMasters = group.SelectMany(topic => topic.SubTopicMasters).ToList()
                                })
                                .OrderBy(st => st.Topic)
                                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(response);
        }
    }

    public class SubTopicsThread
    {
        public string? Topic { get; set; }
        public ICollection<SubTopicMaster>? SubTopicMasters { get; set; }
    }
}
