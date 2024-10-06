using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Security.Claims;
using System.Text;
using Stripe.Checkout;
using Stripe;
using IntelXLWeb.Utilities;

namespace IntelXLWeb.Controllers
{
    public class TutorsController : Controller
    {
        private readonly ILogger<TutorsController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string tutorsUri;
        private readonly string tutorSchedulesUri;
        private readonly string tutorDetailsUri;
        private readonly string? stripeAPIKey;
        private readonly string timingsUri;
        private readonly string daysUri;
        private readonly string tutorTimeConfigsUri;
        private readonly string tutorCertificationsUri;
        private readonly FireBaseStorageConfig _firebaseConfig; 
        private readonly string? baseUri;

        public TutorsController(ILogger<TutorsController> logger, IHttpHandler httpHandler,IConfiguration config, IOptions<FireBaseStorageConfig> firebaseConfig)
        {
            baseUri = config.GetValue<string>("baseUrl");
            _logger = logger;
            _httpHandler = httpHandler;
            tutorsUri = baseUri + IntelXlApiEnum.Tutors;
            tutorSchedulesUri = baseUri + IntelXlApiEnum.TutorSchedules;
            tutorDetailsUri = baseUri + IntelXlApiEnum.TutorDetails;
            stripeAPIKey = config.GetValue<string>("StripeAPIKey");
            StripeConfiguration.ApiKey = stripeAPIKey;
            timingsUri = baseUri + IntelXlApiEnum.Timings;
            daysUri = baseUri + IntelXlApiEnum.Days;
            tutorTimeConfigsUri = baseUri + IntelXlApiEnum.TutorTimeConfigs;
            tutorCertificationsUri = baseUri + IntelXlApiEnum.TutorCertifications;
            _firebaseConfig = firebaseConfig.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllTutors()
        {
            return ViewComponent("AllTutors");
        }
        public async Task<IActionResult> CreateSchedule(int id, string schedulestr)
        {
            AppUser appTutor = new();
            try
            {
                string endpoint = $"{tutorsUri}/GetTutorById/{id}";
                appTutor = await _httpHandler.GetAsync<AppUser>(endpoint);
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                        {
                            new SessionLineItemOptions
                            {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmountDecimal =(decimal)(appTutor.TutorDetails.Pricing*100),
                                //UnitAmountDecimal = 100,
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                Name = "IntelXl",
                                Description = "Tutor booking charge"
                                },
                            },
                            Quantity = 1,
                            },
                        },
                    Mode = "payment",
                    SuccessUrl = Url.Action("Success", "Tutors", new { tutorId = id, schedulestr = schedulestr }, Request.Scheme),
                    CancelUrl = Url.Action("Error", "Tutors", null, Request.Scheme),
                    //SuccessUrl = "https://localhost:44301/Tutors/Success" + "?tutorId=" + id + "&schedulestr=" + schedulestr,
                    //CancelUrl = "https://localhost:44301/Tutors/Error", 

                };

                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> Success(int tutorId, string schedulestr)
        {
            int userId = 0;
            ViewBag.Title = "Payment Success";
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
                var jsonSerializer = new JsonSerializer();
                var reader = new StringReader(schedulestr);
                var studentTutorSchedule = (StudentTutorSchedule)jsonSerializer.Deserialize(reader, typeof(StudentTutorSchedule));
                StudentTutorSchedule tutorSchedule = new StudentTutorSchedule
                {
                    AppUserId = userId,
                    TutorId = tutorId,
                    CreatedDttm = DateTime.Now,
                    ScheduledDate = studentTutorSchedule.ScheduledDate,
                    IsPaid = true,
                    Status = true,
                    FromTimeId = studentTutorSchedule.FromTimeId,
                    ToTimeId = studentTutorSchedule.ToTimeId,
                    TutorTimingConfigId = studentTutorSchedule.TutorTimingConfigId
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(tutorSchedule), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((tutorSchedulesUri), stringContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index", "Tutors");
        }
        public async Task<IActionResult> Error()
        {
            ViewBag.Title = "Payment Failed";
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> AddDetails(TutorDetail tutorDetail)
        {
            bool result = false;
            try
            {
                tutorDetail.Country = "India";
                tutorDetail.TimeZone = DateTimeOffset.Now;
                var stringContent = new StringContent(JsonConvert.SerializeObject(tutorDetail), Encoding.UTF8, "application/json");
                var apiResponse = await _httpHandler.PostAsync((tutorDetailsUri), stringContent);
                result = apiResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return result;
        }
        public async Task<List<TimingMaster>> GetAllTiming()
        {
            List<TimingMaster> result = new List<TimingMaster>();
            try
            {
                result = await _httpHandler.GetAsync<List<TimingMaster>>(timingsUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        public async Task<List<DayMaster>> GetAllDays()
        {
            List<DayMaster> result = new List<DayMaster>();
            try
            {
                result = await _httpHandler.GetAsync<List<DayMaster>>(daysUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        public async Task<bool> AddTimeConfig([FromBody] TimeConfigViewModel timeConfig)
        {
            bool result = false;
            List<TutorTimingConfig> tutorTimingConfig = new List<TutorTimingConfig>();
            try
            {
                int tutorId = timeConfig.TutorId;
                if (timeConfig.TimeConfigs != null && timeConfig.TimeConfigs.Count > 0)
                {
                    foreach (var item in timeConfig.TimeConfigs)
                    {
                        int dayId = item.DayId;
                        if (item.TimeSlots != null && item.TimeSlots.Count > 0)
                        {
                            foreach (var timeSlot in item.TimeSlots)
                            {
                                var config = new TutorTimingConfig
                                {
                                    FromTimeId = timeSlot.FromTimeId,
                                    ToTimeId = timeSlot.ToTimeId,
                                    TutorId = tutorId,
                                    DayId = dayId
                                };
                                tutorTimingConfig.Add(config);
                            }
                        }
                    }
                }
                string endpoint = $"{tutorTimeConfigsUri}/CreateTimeCofig";
                var stringContent = new StringContent(JsonConvert.SerializeObject(tutorTimingConfig), Encoding.UTF8, "application/json");
                using (var response = await _httpHandler.PostAsync(endpoint, stringContent))
                {
                    result = response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
        public async Task<List<TutorTimingConfig>> GetTutorAvailability(int tutorId)
        {
            List<TutorTimingConfig> result = new List<TutorTimingConfig>();
            try
            {
                result = await _httpHandler.GetAsync<List<TutorTimingConfig>>($"{tutorTimeConfigsUri}/GetTutorAvailability/{tutorId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        [HttpPost]
        public async Task<string> Uploadfiles(IFormFile file)
        {
            string filePath = string.Empty;
            try
            {
                //filePath = "https://firebasestorage.googleapis.com/v0/b/myproject-56e06.appspot.com/o/TutorProfiles%2Fwarning.png?alt=media&token=60e0246d-d7c9-4762-9540-81000587cc44";
                if (file!=null && file.Length > 0)
                {
                   filePath = await StorageHelper.UploadFileToStorage(file, _firebaseConfig);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }    
            return filePath;
        }
        public async Task<bool> AddCertification([FromBody] TutorCertification tutorCertification)
        {
            bool result = false;           
            try
            {               
                var stringContent = new StringContent(JsonConvert.SerializeObject(tutorCertification), Encoding.UTF8, "application/json");
                using (var response = await _httpHandler.PostAsync(tutorCertificationsUri, stringContent))
                {
                    result = response.IsSuccessStatusCode;
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
