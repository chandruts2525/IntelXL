using IntelXL.HttpHandler;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Razorpay.Api;

using System.Net;
using System.Security.Claims;
using System.Text;

namespace IntelXLWeb.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration configure;
        private readonly IHttpHandler _httpHandler;
        private readonly string _subscriptionsUri;
        private readonly string _paymentsUri;
        private readonly string _userSubscriptionsUri;
        private readonly string? _razorPayKey;
        private readonly string? _razorPaySecret;
        private readonly string? _couponsUri;
        private readonly int _gstPercentage; 
        private readonly string? baseUri;
        public PaymentController(IConfiguration configuration, IHttpHandler httpHandler, ILogger<PaymentController> logger)
        {
            configure = configuration;
            baseUri = configuration.GetValue<string>("baseUrl");
            _razorPayKey = configure.GetValue<string>("RazorPayKey");
            _razorPaySecret = configure.GetValue<string>("RazorPaySecret");
            _logger = logger;
            _gstPercentage = configure.GetValue<int>("GstPercentage");
            _httpHandler = httpHandler;
            _subscriptionsUri = baseUri + IntelXlApiEnum.Subscription;
            _paymentsUri = baseUri + IntelXlApiEnum.Payments;
            _userSubscriptionsUri = baseUri + IntelXlApiEnum.UserSubscriptions;
            _couponsUri = baseUri + IntelXlApiEnum.Coupons;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PaymentDetail> CreateOrderId(PurchaseDetail purchase)
        {
            PaymentDetail paymentDetail = new PaymentDetail();
            UserSubscription? existingSubscription = new();
            int userId = 0;
            try
            {
                SubscriptionMaster subscription = await GetSubscriptionById(purchase.SubscriptionId);
                CouponMaster coupon = await GetCouponByCode(purchase.Coupon);
                int discount = 0;

                decimal actualAmount = subscription.SubscriptionAmount;

                if (coupon != null)
                    discount = coupon.OfferPercentage;

                if (discount > 0)
                    subscription.SubscriptionAmount = (subscription.SubscriptionAmount) - (subscription.SubscriptionAmount * coupon.OfferPercentage / 100);

                if (_gstPercentage > 0)
                    subscription.SubscriptionAmount = (subscription.SubscriptionAmount) + (subscription.SubscriptionAmount * _gstPercentage / 100);

                if (subscription != null)
                {
                    Dictionary<string, object> input = new Dictionary<string, object>();
                    input.Add("amount", (subscription.SubscriptionAmount * 100));
                    input.Add("currency", subscription.CurrencyType.ToUpper());
                    //input.Add("receipt", "12121");

                    RazorpayClient client = new RazorpayClient(_razorPayKey, _razorPaySecret);

                    Order order = client.Order.Create(input);
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                    if (userIdClaim != null)
                    {
                        userId = int.Parse(userIdClaim.Value);
                    }
                    existingSubscription = await IsSubscriptionExists(userId, purchase.SubscriptionId);
                    paymentDetail = new PaymentDetail()
                    {
                        //Name = ,
                        FinalAmount = subscription.SubscriptionAmount,
                        ActualAmount = actualAmount,
                        Description = purchase.CourseName,
                        Currency = purchase.Currency,
                        FirstName = purchase.FirstName,
                        LastName = purchase.LastName,
                        profileContact = purchase.Mobile,
                        ProfileEmail = purchase.Email,
                        OrderId = order["id"].ToString(),
                        Discount = discount,
                        Gst = _gstPercentage,
                        Duration = subscription.SubscriptionDuration,
                        IsSubscriptionExists = existingSubscription != null && existingSubscription.SubscriptionId == purchase.SubscriptionId ? true : false,                       
                        //    Notes = new Dictionary<string, string>()
                        //{
                        //    { "note 1", "first note while creating order" }, { "note 2", "you can add max 15 notes" },
                        //}
                    };
                    if (coupon != null)
                    {
                        paymentDetail.CouponId = coupon.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
            }
            return paymentDetail;
        }

        public async Task<bool> VerifyAndSavePayment(VerifyAndSavePayment savePayment)
        {
            bool isSuccess = false;
            int userId = 0;

            try
            {
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                Claim userIdClaim = claims.FirstOrDefault(c => c.Type == "UserID");
                if (userIdClaim != null)
                {
                    userId = int.Parse(userIdClaim.Value);
                }

                SubscriptionMaster subscription = await GetSubscriptionById(savePayment.SubscriptionId);

                if (subscription == null)
                    throw new Exception("Error while fetch subscription detail by subscription id");
                savePayment.Amount = subscription.SubscriptionAmount;

                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add("razorpay_payment_id", savePayment?.RazorPaymentId);
                attributes.Add("razorpay_order_id", savePayment?.OrderId);
                attributes.Add("razorpay_signature", savePayment?.Signature);

                Utils.verifyPaymentSignature(attributes);

                savePayment.Status = true;
                var savedPayment = await CreatePaymentEntry(userId, savePayment);

                if (!savedPayment.IsSuccessStatusCode)
                    throw new Exception("Error while save the payment detail after payment");

                var subscribed = await UpdateUser(userId, savePayment.SubscriptionId, savePayment.subscriptionDuration);

                if (!subscribed.IsSuccessStatusCode)
                    throw new Exception("Error while save the subscription detail after payment");
                //var classIdsStr = this.Request.Cookies["ixl_c"];
                var classIdsStr = "";
                if (User != null && User.Identity != null)
                {                    
                    Claim classIdClaim = claims.FirstOrDefault(c => c.Type == "ClassId");
                    if (classIdClaim != null)
                    {
                        classIdsStr = classIdClaim.Value;
                        identity.RemoveClaim(classIdClaim);
                    }
                }

                var classIds = new List<int>();
                if (!string.IsNullOrEmpty(classIdsStr))
                {
                    foreach (var item in classIdsStr.Split(','))
                    {
                        if (int.TryParse(item, out int id))
                        {
                            classIds.Add(id);
                        }
                    }
                }
                if (!classIds.Contains(subscription.ClassId))
                {
                    classIds.Add(subscription.ClassId);
                }
                var newstr = string.Join(",", classIds);              

                identity.AddClaim(new Claim("ClassId", newstr));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                //var cookieOptions = new CookieOptions
                //{
                //    Expires = DateTime.Now.AddMinutes(60),
                //    HttpOnly = true,
                //    Secure = true
                //};
                //this.Response.Cookies.Append("ixl_c", newstr, cookieOptions);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                savePayment.Status = false;
                var savedPayment = await CreatePaymentEntry(userId, savePayment);
                _logger.LogInformation(ex.Message.ToString());
            }
            return isSuccess;
        }

        private async Task<HttpResponseMessage> UpdateUser(int userId, int subscriptionId, int duration)
        {
            UserSubscription? existingSubscription = new();
            DateTime expireDate = DateTime.UtcNow;
            HttpResponseMessage httpResponseMessage=new HttpResponseMessage();
            try
            {
                existingSubscription = await IsSubscriptionExists(userId, subscriptionId);                
                if(existingSubscription != null && existingSubscription.SubscriptionId== subscriptionId)
                {
                    expireDate = existingSubscription.ExpireDttm.AddMonths(duration);
                }
                else
                {
                    expireDate = expireDate.AddMonths(duration);
                }               
                UserSubscription userSubscription = new UserSubscription
                {
                    UserSubscriptionId = existingSubscription != null && existingSubscription.SubscriptionId == subscriptionId ? existingSubscription.UserSubscriptionId : 0,
                    AppUserId = userId,
                    SubscriptionId = subscriptionId,
                    SubscriptionType = "",
                    CreatedDttm = DateTime.UtcNow,
                    ExpireDttm = expireDate,
                    RemaingDays = (int)(expireDate - DateTime.UtcNow).TotalDays
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(userSubscription), Encoding.UTF8, "application/json");
                if (existingSubscription != null && existingSubscription.SubscriptionId == subscriptionId)              
                {
                    
                    httpResponseMessage = await _httpHandler.PutAsync(_userSubscriptionsUri + "/" + userSubscription.UserSubscriptionId, stringContent);
                }
                else
                {
                    httpResponseMessage = await _httpHandler.PostAsync(_userSubscriptionsUri, stringContent);
                }
                
                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<HttpResponseMessage> CreatePaymentEntry(int userId, VerifyAndSavePayment savePayment)
        {
            UserPayment payment = new();
            try
            {
                payment.AppUserId = userId;
                payment.AmountPaid = savePayment.finalAmount;
                payment.InitialAmount = savePayment.ActualAmount;
                payment.Status = savePayment.Status;
                payment.PaymentDate = DateTime.UtcNow;
                payment.FirstName = savePayment.FirstName;
                payment.LastName = savePayment.LastName;
                payment.Contact = savePayment.Contact;
                payment.Email = savePayment.Email;
                payment.ResponsePaymentId = savePayment.RazorPaymentId;
                payment.CouponId= savePayment.CouponId;

                var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
                var result = await _httpHandler.PostAsync(_paymentsUri, stringContent);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<SubscriptionMaster> GetSubscriptionById(int SubscriptionId)
        {

            SubscriptionMaster subscription = new SubscriptionMaster();
            try
            {
                subscription = await _httpHandler.GetAsync<SubscriptionMaster>(_subscriptionsUri + "/" + SubscriptionId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
            }
            return subscription;
        }

        public async Task<CouponMaster> GetCouponByCode(string code)
        {
            CouponMaster? coupon = null;
            try
            {
                coupon = await _httpHandler.GetAsync<CouponMaster>(_couponsUri + "/GetByCouponCode/" + code);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
            }
            return coupon;
        }
        public async Task<UserSubscription> IsSubscriptionExists(int userId,int subscriptionId)
        {
            
            UserSubscription? existingSubscription = new();
            try
            {
                string endpoint = $"{_userSubscriptionsUri}/ValidateSubscription/{userId}?subscriptionId={subscriptionId}";
                using (var response = await _httpHandler.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (responseData != null)
                        {
                            existingSubscription = JsonConvert.DeserializeObject<UserSubscription>(responseData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message.ToString());
            }
            return existingSubscription;
        }
    }
}

