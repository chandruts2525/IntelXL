namespace IntelXLWeb.Models
{
    public class PaymentDetail
    {
        public decimal FinalAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public string Currency { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string profileContact { get; set; }
        public string ProfileEmail { get; set; }
        public int Discount { get; set; }
        public int Gst { get; set; }
        public int Duration { get; set; }
        public bool IsSubscriptionExists { get; set; }
        public int? CouponId { get; set; }
    }
}
