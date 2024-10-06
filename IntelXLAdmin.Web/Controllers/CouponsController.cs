using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using IntelXLDataAccess.Models;

using IntelXLWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Text;

namespace IntelXLAdmin.Web.Controllers
{
    [Authorize]
    public class CouponsController : Controller
    {
        private readonly ILogger<CouponsController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly string _couponsUri;
        private readonly string? baseUri;
        public CouponsController(ILogger<CouponsController> logger, IHttpHandler httpHandler, IConfiguration configuration)
        {
            baseUri = configuration.GetValue<string>("baseUrl");
            _httpHandler = httpHandler;
            _logger = logger;           
            _couponsUri = baseUri + IntelXlApiEnum.Coupons;
            
        }
        public async Task<IActionResult> Index()
        {
            List<CouponMaster> coupons = new List<CouponMaster>();
            try
            {
                coupons = await _httpHandler.GetAsync<List<CouponMaster>>(_couponsUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(coupons);
        }
        public async Task<IActionResult> AddCoupon(CouponMaster coupon)
        {           
            try
            {                
                coupon.StartDate = coupon.StartDate.ToUniversalTime();
                coupon.EndDate= coupon.EndDate.ToUniversalTime();
                var stringContent = new StringContent(JsonConvert.SerializeObject(coupon), Encoding.UTF8, "application/json");
                var response = await _httpHandler.PostAsync(_couponsUri, stringContent);                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index");
        }
        public async Task<bool> UpdateCouponStatus(int id, bool status)
        {
            bool result = false;
            try
            {
                CouponMaster coupon = new();
                coupon = await _httpHandler.GetAsync<CouponMaster>(_couponsUri + "/" + id);
                coupon.Status = status;
                var stringContent = new StringContent(JsonConvert.SerializeObject(coupon), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await _httpHandler.PutAsync(_couponsUri + "/" + id, stringContent);
                result = httpResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return result;
        }
        public async Task<IActionResult> Edit(int id)
        {
            CouponMaster coupon = new CouponMaster();
            try
            {
                coupon = await _httpHandler.GetAsync<CouponMaster>(_couponsUri +"/"+ id);                
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return Json(new { coupon = coupon,startDate= coupon.StartDate.ToLocalTime().ToString("yyyy-MM-dd"),endDate= coupon.EndDate.ToLocalTime().ToString("yyyy-MM-dd") });
        }
        public async Task<bool> UpdateCoupon(CouponMaster coupon)
        {
            bool result = false;
            try
            {
                coupon.StartDate = coupon.StartDate.ToUniversalTime();
                coupon.EndDate = coupon.EndDate.ToUniversalTime();
                var stringContent = new StringContent(JsonConvert.SerializeObject(coupon), Encoding.UTF8, "application/json");
                var response = await _httpHandler.PutAsync(_couponsUri +"/" + coupon.Id, stringContent);
                result = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return result;
        }
    }
}
