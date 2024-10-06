namespace IntelXLWeb.Models
{
    public class VerifyAndSavePayment
    {
        public string RazorPaymentId { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public int SubscriptionId { get; set; }
        public decimal finalAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public int subscriptionDuration { get; set; }
        public int? CouponId { get; set; }
    }
}
