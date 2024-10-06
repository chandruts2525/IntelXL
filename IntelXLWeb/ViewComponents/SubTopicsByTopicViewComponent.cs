using IntelXL.HttpHandler;
using IntelXLDataAccess.Models;
using IntelXLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Net;
using System.Text;

namespace IntelXLWeb.ViewComponents
{
    public class SubTopicsByTopicViewComponent : ViewComponent
    {
        private readonly ILogger<SubTopicsByTopicViewComponent> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string? _coursesUri; 
        private readonly string? baseUri;
        public SubTopicsByTopicViewComponent(ILogger<SubTopicsByTopicViewComponent> logger,IHttpHandler httpHandler,IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            _coursesUri = baseUri + IntelXlApiEnum.Courses; 
            _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync(int courseId, string subjectName, string topicName)
        {
            var topics = new List<string>();
            List<string?> subjects = new List<string?>();
            var response = new List<ClassesWithSubTopics>();

            try
            {
                string decodedString = WebUtility.HtmlDecode(subjectName);

                byte[] bytes = Encoding.Unicode.GetBytes(decodedString);

                subjectName = Encoding.Unicode.GetString(bytes);
                var course = await _httpHandler.GetAsync<CourseMaster>($"{_coursesUri}/GetByIdIncludeAllAsync/{courseId}");

                if (course != null && course.CourseId > 0)
                {
                    topics = course.ClassMasters
                        .SelectMany(c => c.SubjectMasters)
                        .Where(s => s.SubjectName == subjectName)
                        .SelectMany(s => s.UnitMasters)
                        .SelectMany(s => s.TopicMasters)
                        .OrderBy(t=>t.Order)
                        .Select(t => t.Topic)
                        .Distinct()
                        .ToList();

                    subjects = course.ClassMasters
                            .SelectMany(classItem => classItem.SubjectMasters)
                            .Select(subject => subject.SubjectName)
                            .Distinct()
                            .ToList();

                    response = course.ClassMasters
                        .Select(c => new ClassesWithSubTopics
                        {
                            ClassMaster = new ClassMaster
                            {
                                ClassName = c.ClassName,
                                ClassId = c.ClassId
                            },
                            SubTopicMasters = c?.SubjectMasters?
                                        .SelectMany(s => s.UnitMasters)
                                        .SelectMany(s => s.TopicMasters)
                                        .Where(t => t.Topic == topicName)
                                        .SelectMany(t => t.SubTopicMasters)
                                        .OrderBy(st=>st.Order)
                                        .ToList()
                        })
                        .ToList();
                }

                ViewBag.Topics = topics;
                ViewBag.Subjects = subjects;
                ViewBag.Topic = topicName;
                ViewBag.Subject = subjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return View(response);
        }
    }

    public class ClassesWithSubTopics
    {
        public ClassMaster? ClassMaster { get; set; }
        public List<SubTopicMaster>? SubTopicMasters { get; set; }
    }
}
