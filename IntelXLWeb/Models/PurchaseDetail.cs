namespace IntelXLWeb.Models
{
    public class PurchaseDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int SubscriptionId { get; set; }
        public string Coupon { get; set; }
    }
}
