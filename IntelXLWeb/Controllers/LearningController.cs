using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace IntelXLWeb.Controllers
{
    public class LearningController : Controller
    {
        private readonly string? baseUri;
        public readonly string _questionUri;
        public readonly string _subjectsUri;
        public readonly string _coursesUri;
        public readonly string _classUri;
        public readonly IHttpHandler _httpHandler;
        public readonly ILogger<LearningController> _logger;
        private readonly string _subscriptionsUri;
        private readonly string _userExamsUri;
        private readonly int _courseId;
        private readonly string? _subjectName;
        //private readonly int _maxFreeCount;

        public LearningController(ILogger<LearningController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _questionUri = baseUri + IntelXlApiEnum.Questions;
            _subjectsUri = baseUri + IntelXlApiEnum.Subjects;
            _coursesUri = baseUri + IntelXlApiEnum.Courses;
            _classUri = baseUri + IntelXlApiEnum.Classes;
            _subscriptionsUri = baseUri + IntelXlApiEnum.Subscription;
            _userExamsUri = baseUri + IntelXlApiEnum.UserExams;
            _httpHandler = httpHandler;
            _logger = logger;
            _courseId = configuration.GetValue<int>("CourseId");
            _subjectName = configuration.GetValue<string>("SubjectName");
            //_maxFreeCount = configuration.GetValue<int>("MaxFreeQuestions");
        }

        public async Task<IActionResult> Index(int courseId, string subject, string viewBy)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Claim ipAddressClaim = claims.FirstOrDefault(c => c.Type == "IPAddress");
            string ipAddressValue = "";

            if (ipAddressClaim != null)
            {
                ipAddressValue = ipAddressClaim.Value;
            }
            var currentIp = await GetIpAddress();
            if (!string.IsNullOrEmpty(ipAddressValue) && ipAddressValue != currentIp)
            {
                return RedirectToAction("SessionExpired", "Home");
            }
            subject = DecodeSubject(subject);
            ArrayList arrayList = new ArrayList { courseId, subject, viewBy };
            ViewBag.Data = arrayList;
            return View();
        }

        public IActionResult GetSubjects(int courseId, string subject, string viewBy)
        {
            subject = DecodeSubject(subject);
            return ViewComponent("ClassesBysubject", new { courseId = courseId, subject = subject, viewName = viewBy });
        }


        public async Task<IActionResult> SubTopicsByClass(int courseId, int classId, string subName)
        {
            subName = DecodeSubject(subName);
            ArrayList arrayList = new ArrayList { courseId, classId, subName };
            ViewBag.Data = arrayList;
            return View();
        }

        public IActionResult GetSubTopicsByClass(int courseId, int classId, string subName)
        {
            subName = DecodeSubject(subName);
            return ViewComponent("SubTopicsByClass", new { courseId = courseId, classId = classId, subject = subName });
        }

        public IActionResult Topics(int courseId, string subjectName)
        {
            subjectName = DecodeSubject(subjectName);
            ViewBag.CourseId = (courseId < 1) ? _courseId : courseId;
            ViewBag.Subject = string.IsNullOrWhiteSpace(subjectName) ? _subjectName : subjectName;
            return View();
        }

        public async Task<IActionResult> GetTopics(int courseId, string subjectName)
        {
            subjectName = DecodeSubject(subjectName);
            return ViewComponent("TopicsBySubject", new { courseId = courseId, subjectName = subjectName });
        }

        public async Task<IActionResult> SubTopics(int courseId, string subjectName, string topicName)
        {
            subjectName = DecodeSubject(subjectName);
            ArrayList arrayList = new ArrayList { courseId, subjectName, topicName };
            ViewBag.Data = arrayList;
            return View();
        }

        public async Task<IActionResult> GetSubTopics(int courseId, string subjectName, string topicName)
        {
            topicName = DecodeSubject(topicName);
            subjectName = DecodeSubject(subjectName);
            return ViewComponent("SubTopicsByTopic", new { courseId = courseId, subjectName = subjectName, topicName = topicName });
        }
        public string DecodeSubject(string subjectName)
        {
            string decodedString = WebUtility.HtmlDecode(subjectName);

            byte[] bytes = Encoding.Unicode.GetBytes(decodedString);

            string subject = Encoding.Unicode.GetString(bytes);
            return subject;
        }


        public async Task<IActionResult> GetTopicsFromSubject(int id)
        {
            SubjectMaster subject = new SubjectMaster();
            try
            {
                subject = await _httpHandler.GetAsync<SubjectMaster>($"{_subjectsUri}/GetByIdWithSubTopics/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Json(subject);
        }

        public IActionResult Practice(int subTopicId,int classId)
        {
            int userId = 0;
            ViewBag.SubTopicId = subTopicId;
            try
            {
                if (User != null && User.Identity != null)
                {
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (userIdClaim != null)
                    {
                        userId = int.Parse(userIdClaim.Value);
                    }
                }
                ViewBag.UserId = userId;
                ViewBag.ClassId = classId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }

        public async Task<IActionResult> GetPracticeQuestions(int subTopicId, int classId = 0, int userId = 0)
        {
            bool hasSubscription = false;
            List<QuestionMaster>? questions = new List<QuestionMaster>();
            ClassMaster classMaster = new();
            int count = 0;
            var classIdsStr = "";
            List<int> classIds = new List<int>();
            int trialcount = 0;
            try
            {
                //var response = await _httpHandler.GetAsync<ArrangedQuestions>($"{_questionUri}/GetPracticeQuestions/{subTopicId}?userId={userId}");
                // questions=response.Questions;
                // count = response.TotalCount;                
                using (var response = await _httpHandler.GetAsync($"{_questionUri}/GetPracticeQuestions/{subTopicId}?userId={userId}&isSignedIn={User.Identity.IsAuthenticated}"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            var content = JsonConvert.DeserializeObject<ArrangedQuestions>(responseData);
                            count = content.TotalCount;
                            questions = content?.Questions;
                            trialcount = content.TrialCount;
                        }
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

                //classIdsStr = this.Request.Cookies["ixl_c"];

                if( !string.IsNullOrEmpty(classIdsStr))
                {
                    var splitItems = classIdsStr.Split(',');
                    foreach (var item in splitItems)
                    {
                        if (int.TryParse(item, out int id))
                        {
                            classIds.Add(id);
                        }
                    }
                }
                if (classIds.Contains(classId))
                {
                    hasSubscription = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            //return Json(questions);
            return Json(new { questions = questions, hasSubscription = hasSubscription, count = count,trialCount= trialcount });
        }

        public IActionResult Certificate(string score, string time, string ans, int subTopicId, int noOfQuestions,int classId)
        {
            ViewBag.SubTopicId = subTopicId;
            ViewBag.ClassId = classId;
            if (score != null)
                ViewBag.Score = score;

            if (ans != null)
                ViewBag.Result = $"{ans} / {noOfQuestions}";

            if (time != null)
            {
                var times = time.Split(':');

                if (times.Length >= 3)
                {
                    int.TryParse(times[1], out int min);
                    int.TryParse(times[2], out int sec);

                    ViewBag.Time = (min > 0) ? $"{min} min {sec} sec" : $"{sec} sec";
                }
            }
            return View();
        }

        public async Task<IActionResult> GetClasses(int id)
        {
            List<ClassMaster>? classMasters = new();
            try
            {
                HttpResponseMessage response = await _httpHandler.GetAsync(_classUri + "/GetAllWithSubTopics/" + id);
                var responseContent = await response.Content.ReadAsStringAsync();
                classMasters = JsonConvert.DeserializeObject<List<ClassMaster>>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return Json(classMasters);
        }
        //public IActionResult GetFreeCount()
        //{
        //    return Json(new { count = _maxFreeCount });
        //}
        public async Task<string> GetIpAddress()
        {
            string ipAddress = string.Empty;

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return ipAddress;
        }
        public async Task<bool> UserExamEntry(UserExam userExam)
        {
            bool result = false;
            HttpResponseMessage httpResponseMessage;
            try
            {
                if (User != null && User.Identity != null)
                {
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (userIdClaim != null)
                    {
                        userExam.AppUserId = int.Parse(userIdClaim.Value);
                        userExam.CreatedDttm = DateTime.UtcNow;
                        var existingQuestion = await IsQuestionExists(userExam.AppUserId, userExam.QuestionId, userExam.PracticeType,userExam.YearOfQuestion);
                        userExam.UserExamId = existingQuestion?.UserExamId ?? 0;
                        var stringContent = new StringContent(JsonConvert.SerializeObject(userExam), Encoding.UTF8, "application/json");
                        if (existingQuestion!=null && existingQuestion.UserExamId != 0)
                        {                            
                            httpResponseMessage = await _httpHandler.PutAsync(_userExamsUri + "/" + existingQuestion.UserExamId, stringContent);
                            result = httpResponseMessage.IsSuccessStatusCode;
                        }
                        else
                        {
                            httpResponseMessage = await _httpHandler.PostAsync(_userExamsUri, stringContent);
                            result = httpResponseMessage.IsSuccessStatusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<IActionResult> PracticeAgain(int subTopicId, int classId,int userId = 0)
        {
            try
            {
                if (userId != 0)
                {
                    await _httpHandler.DeleteAsync($"{_userExamsUri}/DeleteRange/{subTopicId}?userId={userId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Practice", new { subTopicId, classId });
        }
        public async Task<IActionResult> PracticeAgainByYear(int subjectId, string year,bool isPrevious, int userId = 0)
        {
            var returnAction = "";
            var type = "";
            try
            {
                if (isPrevious)
                {
                    returnAction = "PracticePreviousQuestions";
                    type = "previous";
                }
                else
                {
                    returnAction = "PracticeProbableQuestions";
                    type = "probable";
                }
                if (userId != 0)
                {
                    await _httpHandler.DeleteAsync($"{_userExamsUri}/DeletePreviousRange/{subjectId}?userId={userId}&type={type}&year={year}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction(returnAction, new { subjectId = subjectId, year = year });
        }
        public IActionResult PracticePreviousQuestions(int subjectId, string year)
        {
            int classId = 0;
            int userId = 0;

            try
            {
                if (User != null && User.Identity != null)
                {
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (userIdClaim != null)
                    {
                        userId = int.Parse(userIdClaim.Value);
                    }
                }
                ViewBag.UserId = userId;
                ViewBag.SubjectId = subjectId;
                ViewBag.Year = year;
                ViewBag.IsPrevious = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }
        public async Task<IActionResult> GetPreviousQuestions(int subjectId, string year, int userId = 0)
        {
            List<QuestionMaster>? questions = new List<QuestionMaster>();
            int count = 0;
            try
            {
                string endpoint = $"{_questionUri}/GetPracticeQuestionsbyYear/{subjectId}?year={year}&userId={userId}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            //questions = JsonConvert.DeserializeObject<List<QuestionMaster>>(responseData);
                            var content = JsonConvert.DeserializeObject<ArrangedQuestions>(responseData);
                            count = content.TotalCount;
                            questions = content?.Questions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            //return Json(new { questions = questions });
            return Json(new { questions = questions, count = count });
        }
        public IActionResult PracticeProbableQuestions(int subjectId, string year)
        {
            int classId = 0;
            int userId = 0;

            try
            {
                if (User != null && User.Identity != null)
                {
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (userIdClaim != null)
                    {
                        userId = int.Parse(userIdClaim.Value);
                    }
                }
                ViewBag.UserId = userId;
                ViewBag.SubjectId = subjectId;
                ViewBag.Year = year;
                ViewBag.IsPrevious = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View("PracticePreviousQuestions");
        }
        public async Task<IActionResult> GetProbableQuestions(int subjectId, string year, int userId = 0)
        {
            List<QuestionMaster>? questions = new List<QuestionMaster>();
            int count = 0;
            try
            {
                string endpoint = $"{_questionUri}/GetProbableQuestionsbyYear/{subjectId}?year={year}&userId={userId}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            //questions = JsonConvert.DeserializeObject<List<QuestionMaster>>(responseData);
                            var content = JsonConvert.DeserializeObject<ArrangedQuestions>(responseData);
                            count = content.TotalCount;
                            questions = content?.Questions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            //return Json(new { questions = questions });
            return Json(new { questions = questions, count = count });
        }
        public async Task<UserExam> IsQuestionExists(int userId, int questionId, string type,string year)
        {
            UserExam? userExam = new();
            try
            {
                string endpoint = $"{_userExamsUri}/IsQuestionExists/{userId}?questionId={questionId}&type={type}&year={year}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            userExam = JsonConvert.DeserializeObject<UserExam>(responseData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
            }
            return userExam;
        }

    }
}
