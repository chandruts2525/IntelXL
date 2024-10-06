using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class UserPayment
{
    [JsonPropertyName("paymentId")]
    public int PaymentId { get; set; }

    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }

    [JsonPropertyName("couponId")]
    public int? CouponId { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; }
    [JsonPropertyName("initialAmount")]
    public decimal InitialAmount { get; set; }

    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonPropertyName("paymentDate")]
    public DateTime PaymentDate { get; set; }

    [JsonPropertyName("responsePaymentId")]
    public string? ResponsePaymentId { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("contact")]
    public string? Contact { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("recurringMethod")]
    public string RecurringMethod { get; set; } = "Month";

    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }
    public virtual CouponMaster? Coupon { get; set; }
}
public class PagedPayments
{
    public List<UserPayment>? Payments { get; set; }
    public int TotalPages { get; set; }
}
