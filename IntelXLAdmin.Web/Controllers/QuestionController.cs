using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly IConfiguration _configuration;
        private readonly string? questionUri;
        private readonly string? courseUri;
        private readonly string? answerUri;
        private readonly string? choiceUri;
        private readonly string languageOfInstructionsUri;
        private readonly int pageSize;
        private int _userId;
        private readonly string? baseUri;
        public QuestionController(ILogger<QuestionController> logger, IHttpHandler httpHandler, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            _configuration = configuration;
            questionUri = baseUri + IntelXlApiEnum.Questions;
            courseUri = baseUri + IntelXlApiEnum.Courses;
            answerUri = baseUri + IntelXlApiEnum.Answers;
            choiceUri = baseUri + IntelXlApiEnum.Choices;
            languageOfInstructionsUri = baseUri + IntelXlApiEnum.LanguageOfInstructions;
            _logger = logger;
            pageSize = _configuration.GetValue<int>("PageSize");
            ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _userId = userId;
            }
        }
        public async Task<IActionResult> Index()
        {
            List<LanguageOfInstructionMaster> languages = await GetLanguages();
            languages = languages.Where(lang => lang.Status).ToList(); 
            ViewBag.languages = new SelectList(languages, "LanguageId", "Language");           
            return View();
        }

        [HttpGet]
        public IActionResult AllQuestions(int id, int page = 1,bool status=true)
        {
            //List<QuestionMaster>? result = new List<QuestionMaster>();
            ViewBag.SubTopicId = id;
            ViewBag.CurrentPage = page;
            ViewBag.ItemsPerPage = pageSize;
            ViewBag.Status = status;
            //try
            //{
            //    string endpoint = $"{questionUri}/GetQuestions/{id}?pageNum={page}";
            //    using (var response = await _httpHandler.GetAsync(endpoint))
            //    {
            //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //        {
            //            var responseData = await response.Content.ReadAsStringAsync();
            //            if (responseData != null)
            //            {
            //                var content = JsonConvert.DeserializeObject<ArrangedQuestions>(responseData);
            //                ViewBag.TotalPages = content?.TotalCount;
            //                result = content?.Questions;
            //            }
            //        }
            //    }

                // int skipCount = (page - 1) * pageSize;
                // result = await _httpHandler.GetAsync<List<QuestionMaster>>(questionUri + "/GetAllQuestions/" + id);
                // int totalPages = (int)Math.Ceiling(result.Count / (double)pageSize);
                // ViewBag.TotalPages = totalPages;
                // result = result.Skip(skipCount)
                //.Take(pageSize).ToList();

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message.ToString());
            //}
            return View(/*result*/);
        }

        public async Task<IActionResult> EditQuestion(int id)
        {
            QuestionMaster question = new QuestionMaster();

            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + id);
            return View(question);
        }


        [HttpPut]
        public async Task<bool> EditQuestion([FromBody] QuestionMaster question)
        {
            bool result = false;
            try
            {
                question.UpdatedDttm= DateTime.UtcNow;
                question.UpdatedBy = _userId;
                
                var questionStringContent = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");

                var questionResponse = await _httpHandler.PutAsync($"{questionUri}/{question.QuestionId}", questionStringContent);
                question.Answer.UpdatedDttm = DateTime.UtcNow;
                question.Answer.UpdatedBy = _userId;
                var answerStringContent = new StringContent(JsonConvert.SerializeObject(question.Answer), Encoding.UTF8, "application/json");
                var answerResponse = await _httpHandler.PutAsync($"{answerUri}/{question.AnswerId}", answerStringContent);                
                await _httpHandler.DeleteAsync($"{choiceUri}/DeleteRange/{question.QuestionId}");
                var choiceUpdateTasks = question.ChoiceMasters.Select(item =>
                {
                    item.UpdatedDttm = DateTime.UtcNow;
                    item.UpdatedBy = _userId;
                    var choiceStringContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                if (item.ChoiceId != 0)
                {
                    return _httpHandler.PutAsync($"{choiceUri}/{item.ChoiceId}", choiceStringContent);
                }
                else
                {
                    return _httpHandler.PostAsync($"{choiceUri}", choiceStringContent);
                    }
                });
                var choiceResponse=await Task.WhenAll(choiceUpdateTasks);
                if (questionResponse.IsSuccessStatusCode && answerResponse.IsSuccessStatusCode && choiceResponse.All(task => task.IsSuccessStatusCode))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }


        public async Task<bool> DeleteQuestion(int id)
        {
            QuestionMaster question = new QuestionMaster();

            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + id);
            question.Status = false;
            question.IsVerified = false;
            question.UpdatedDttm= DateTime.UtcNow;
            question.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(questionUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> RestoreQuestion(int id)
        {
            QuestionMaster question = new QuestionMaster();

            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + id);
            question.Status = true;
            question.IsVerified = false;
            question.UpdatedDttm= DateTime.UtcNow;
            question.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(questionUri + "/" + id, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }


        public async Task<List<CourseMaster>> GetCourses(int id)
        {
            var courses = await _httpHandler.GetAsync<List<CourseMaster>>(courseUri + "/GetAllCourse/" + id);
            return courses;
        }
        public async Task<List<CourseMaster>> GetAllCourses()
        {
            var courses = await _httpHandler.GetAsync<List<CourseMaster>>(courseUri);
            return courses;
        }
        public async Task<List<LanguageOfInstructionMaster>> GetLanguages()
        {
            List<LanguageOfInstructionMaster> languages = await _httpHandler.GetAsync<List<LanguageOfInstructionMaster>>(languageOfInstructionsUri);
            return languages;
        }

        public IActionResult AddQuestion(int id)
        {
            ViewBag.SubTopicId = id;
            return View();
        }

        [HttpPost]
        public async Task<bool> AddQuestion([FromBody] QuestionMaster questionStr)
        {
            bool result = false;
            try
            {
                questionStr.CreatedDttm = DateTime.UtcNow;
                if (questionStr.Answer != null) { 
                questionStr.Answer.CreatedDttm = DateTime.UtcNow;
                    questionStr.Answer.CreatedBy = _userId;
                }
                questionStr.CreatedBy = _userId;                
                foreach (var item in questionStr.ChoiceMasters)
                {
                    item.CreatedDttm = DateTime.UtcNow;
                    item.CreatedBy = _userId;
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(questionStr), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((questionUri), stringContent);
                result = apiResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<bool> VerifyQuestion(int questionId)
        {
            QuestionMaster question = new QuestionMaster();
            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + questionId);
            question.IsVerified = true;
            question.UpdatedDttm = DateTime.UtcNow;
            question.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(questionUri + "/" + questionId, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<bool> UnVerifyQuestion(int questionId)
        {
            QuestionMaster question = new QuestionMaster();
            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + questionId);
            question.IsVerified = false;
            question.UpdatedDttm = DateTime.UtcNow;
            question.UpdatedBy = _userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(questionUri + "/" + questionId, stringContent);
            return httpResponseMessage.IsSuccessStatusCode;
        }
        public async Task<IActionResult> CloneQuestion(int id)
        {
            QuestionMaster question = new QuestionMaster();
            question = await _httpHandler.GetAsync<QuestionMaster>(questionUri + "/GetQuestionById/" + id);
            List<LanguageOfInstructionMaster> languages = await GetLanguages();
            ViewBag.languages = new SelectList(languages, "LanguageId", "Language");
            return View(question);
        }
       
        public async Task<bool> CheckQuestionExists(int id,string questionstr,int type)
        {
            List<QuestionMaster> questions = new List<QuestionMaster>();
            bool result = false;
            try
            {
                string endpoint = $"{questionUri}/GetAllQuestions/{id}";
                if(type == 2)
                {
                    questionstr = Utilities.Utilities.HTMLToText(questionstr);
                }
                questions = await _httpHandler.GetAsync<List<QuestionMaster>>(endpoint);  
                foreach(var item in questions)
                {
                    if(item.QuestionType==2)
                    {
                        item.Question= Utilities.Utilities.HTMLToText(item.Question);                        
                    }
                    if (item.Question== questionstr)
                    {
                        result = true;
                        break;
                    }
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
    }
}


