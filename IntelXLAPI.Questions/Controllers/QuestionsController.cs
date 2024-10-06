using IntelXLDataAccess.Data;
using IntelXLDataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelXLAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : GenericController<QuestionMaster>
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IntelxlContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly int pageSize;
        private readonly int practiceQuestionsLimit;
        private readonly int guestTrialLimit;

        public QuestionsController(ILogger<QuestionsController> logger, IntelxlContext context, IWebHostEnvironment hostingEnvironment, IConfiguration configuration) : base(context)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            pageSize = _configuration.GetValue<int>("PageSize");
            practiceQuestionsLimit = 30;
            //practiceQuestionsLimit = _configuration.GetValue<int>("PracticeQuestionsLimit");
            guestTrialLimit = _configuration.GetValue<int>("GuestTrialLimit");
        }

        [HttpGet("GetAllQuestions/{id}")]
        public async Task<IActionResult> GetAllQuestions(int id)
        {
            IEnumerable<QuestionMaster> questions = new List<QuestionMaster>();
            try
            {
                questions = await _context.QuestionMasters
                .Where(q => q.SubTopicId == id && q.Status == true)
                .Include(q => q.Answer)
                .Include(q => q.ChoiceMasters)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(questions);
        }

        [HttpGet("GetQuestionById/{id}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var question = new QuestionMaster();
            try
            {
                question = await _context.QuestionMasters
                     .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .Include(q => q.SubTopic)
                    .ThenInclude(st => st.Topic)
                    .ThenInclude(t => t.Unit)
                    .ThenInclude(u => u.Subject)
                    .ThenInclude(s => s.Class)
                    .FirstOrDefaultAsync(q => q.QuestionId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Ok(question);
        }

        //[HttpGet("GetPracticeQuestions/{id}")]
        //public async Task<IActionResult> GetPracticeQuestions(int id)
        //{
        //    var questions = new List<QuestionMaster>();
        //    try
        //    {
        //        questions = await _context.QuestionMasters
        //            .Where(q => q.Status == true && q.SubTopicId == id)
        //            .Include(a => a.Answer)
        //            .Include(c => c.ChoiceMasters)
        //            .ToListAsync();


        //        //string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "SampleJsonDatas");
        //        //string jsonFilePath = Path.Combine(folderPath, "Practice questions.json");
        //        //string jsonData = await System.IO.File.ReadAllTextAsync(jsonFilePath);
        //        //questions = JsonConvert.DeserializeObject<List<QuestionMaster>>(jsonData);
        //        var random = new Random();


        //        var randomizedQuestions = questions?.OrderBy(r => random.Next())
        //       .Take(practiceQuestionsLimit);

        //        return Ok(randomizedQuestions);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message.ToString());
        //    }
        //    return Ok(questions);
        //}

        [HttpGet("GetPracticeQuestions/{id}")]
        public async Task<IActionResult> GetPracticeQuestions(int id, [FromQuery] int userId, bool isSignedIn = false)
        {
            IEnumerable<QuestionMaster> questions = new List<QuestionMaster>();
            int totalQuestionsCount = 0;
            int trialCount = 0;
            try
            {
                var allItems = await _context.QuestionMasters
                    .Where(q => q.Status && q.SubTopicId == id)
                    .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .ToListAsync();
                totalQuestionsCount = allItems.Count();
                questions = allItems
                    .Where(q => !_context.UserExams.Any(ue => ue.QuestionId == q.QuestionId &&
                    ue.AppUserId == userId && ue.PracticeType == "practice" &&
                    ue.AnsweredStatus == 1))
                    .Take(practiceQuestionsLimit)
                    .ToList();
                trialCount = isSignedIn ? practiceQuestionsLimit : guestTrialLimit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching practice questions.");
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { Questions = questions, TotalCount = totalQuestionsCount, TrialCount = trialCount });
        }


        [HttpGet("GetQuestions/{id}")]
        public async Task<IActionResult> GetQuestions(int id, /*[FromQuery]*/ int pageNum, [FromQuery] bool status)
        {
            IEnumerable<QuestionMaster> questions = new List<QuestionMaster>();
            int totalPages = 0;
            try
            {
                //var allItems = await _context.QuestionMasters.Where(q => q.SubTopicId == id && q.Status == status)
                //    .Include(q => q.Answer)
                //    .Include(q => q.ChoiceMasters)
                //    .ToListAsync();
                //totalPages = (int)Math.Ceiling(allItems.Count() / (double)pageSize);
                //questions = allItems
                //    .Skip((pageNum - 1) * pageSize)
                //    .Take(pageSize)
                //    .ToList();

                var allItems = _context.QuestionMasters.AsQueryable();
                totalPages = (int)Math.Ceiling(await allItems.Where(q => q.SubTopicId == id && q.Status == status).CountAsync() / (double)pageSize);
                questions = await allItems
                    .Where(q => q.SubTopicId == id && q.Status == status)
                    .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { TotalCount = totalPages, Questions = questions });
        }
        [HttpGet("GetPracticeQuestionsbyYear/{id}")]
        public async Task<IActionResult> GetPracticeQuestionsByYear(int id, [FromQuery] int userId, [FromQuery] string year)
        {
            int totalQuestionsCount = 0;
            IEnumerable<QuestionMaster> questions = new List<QuestionMaster>();
            try
            {
                questions = await _context.SubjectMasters
                    .Where(q => q.SubjectId == id && q.Status)
                    .SelectMany(u => u.UnitMasters.Where(u => u.Status))
                    .SelectMany(t => t.TopicMasters.Where(t => t.Status))
                    .SelectMany(st => st.SubTopicMasters.Where(t => t.Status))
                    .SelectMany(st => st.QuestionMasters)
                    .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .Where(q => q.Status && q.IsPreviousYearQuestion && q.PreviousYear != null && q.PreviousYear.Contains(year))
                    .ToListAsync();
                totalQuestionsCount = questions.Count();
                questions = questions.Where(q => !_context.UserExams.Any(ue => ue.QuestionId == q.QuestionId &&
                    ue.AppUserId == userId && ue.PracticeType == "previous" &&
                    ue.AnsweredStatus == 1 && ue.YearOfQuestion == year)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching practice questions.");
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { Questions = questions, TotalCount = totalQuestionsCount });
        }
        [HttpGet("GetAllYearsBySubject/{subjectId}")]
        public async Task<IActionResult> GetAllYearsBySubject(int subjectId)
        {
            List<string> years = new List<string>();
            try
            {
                List<string?> allItems = await _context.SubjectMasters
                    .SelectMany(s => s.UnitMasters)
                    .SelectMany(u => u.TopicMasters)
                    .SelectMany(t => t.SubTopicMasters)
                    .SelectMany(s => s.QuestionMasters)
                    .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .Where(q => q.IsPreviousYearQuestion && q.Status && q.PreviousYear != null)
                    .Select(row => row.PreviousYear)
                    .ToListAsync();
                foreach (var item in allItems)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var splitItems = item.Split(',');
                        years.AddRange(splitItems);
                    }
                }
                years = years.Distinct().OrderBy(year => year).ToList();
                return Ok(years);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching previous years.");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("Years/{classId}")]
        public async Task<IActionResult> Years(int classId)
        {
            List<string> years = new List<string>();
            try
            {
                List<string?> allItems = await _context.ClassMasters
                    .Where(c => c.ClassId == classId)
                    .SelectMany(s => s.SubjectMasters)
                    .SelectMany(s => s.UnitMasters)
                    .SelectMany(u => u.TopicMasters)
                    .SelectMany(t => t.SubTopicMasters)
                    .SelectMany(s => s.QuestionMasters)
                    .Where(q => q.IsPreviousYearQuestion && q.Status && q.PreviousYear != null)
                    .Select(q => q.PreviousYear)
                    .ToListAsync();
                foreach (var item in allItems)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var splitItems = item.Split(',');
                        years.AddRange(splitItems);
                    }
                }
                years = years.Distinct().OrderBy(year => year).ToList();

                return Ok(years);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching previous years.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetProbableQuestionsbyYear/{id}")]
        public async Task<IActionResult> GetProbableQuestionsbyYear(int id, [FromQuery] int userId, [FromQuery] string year)
        {
            int totalQuestionsCount = 0;
            IEnumerable<QuestionMaster> questions = new List<QuestionMaster>();
            try
            {
                questions = await _context.SubjectMasters
                    .Where(q => q.SubjectId == id && q.Status)
                    .SelectMany(u => u.UnitMasters.Where(u => u.Status))
                    .SelectMany(t => t.TopicMasters.Where(t => t.Status))
                    .SelectMany(st => st.SubTopicMasters.Where(t => t.Status))
                    .SelectMany(st => st.QuestionMasters)
                    .Include(q => q.Answer)
                    .Include(q => q.ChoiceMasters)
                    .Where(q => q.Status && q.IsProbable && q.ProbableYear != null && q.ProbableYear.Contains(year))
                    .ToListAsync();
                totalQuestionsCount = questions.Count();
                questions = questions.Where(q => !_context.UserExams.Any(ue => ue.QuestionId == q.QuestionId &&
                    ue.AppUserId == userId && ue.PracticeType == "probable" &&
                    ue.AnsweredStatus == 1 && ue.YearOfQuestion == year)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Probable Questions.");
                return StatusCode(500, "Internal Server Error");
            }
            return Ok(new { Questions = questions, TotalCount = totalQuestionsCount });
        }
    }
}
