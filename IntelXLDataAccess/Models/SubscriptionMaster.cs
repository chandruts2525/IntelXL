using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class SubscriptionMaster
{
    [JsonPropertyName("subscriptionId")]
    public int SubscriptionId { get; set; }
    [JsonPropertyName("subscriptionName")]
    public string? SubscriptionName { get; set; }
    [JsonPropertyName("subscriptionDetails")]
    public string? SubscriptionDetails { get; set; }
    [JsonPropertyName("subscriptionAmount")]
    public decimal SubscriptionAmount { get; set; }
    [JsonPropertyName("currencyType")]
    public string? CurrencyType { get; set; }
    [JsonPropertyName("subscriptionDuration")]
    public int SubscriptionDuration { get; set; }
    [JsonPropertyName("coupon")]
    public string? Coupon { get; set; }
    //[JsonPropertyName("subjectCode")]
    //public int? SubjectCode { get; set; }
    [JsonPropertyName("createdBy")]
    public int? CreatedBy { get; set; }
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [JsonPropertyName("classId")]
    public int ClassId { get; set; }
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("course")]
    public virtual CourseMaster? Course { get; set; }
    [JsonPropertyName("class")]
    public virtual ClassMaster? Class { get; set; }
    //[JsonPropertyName("appUsers")]
    //public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>(); 
    [JsonPropertyName("userSubscriptions")]
    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();

    public DateTime? CreatedDttm { get; set; }

    public int? UpdatedBy { get; set; }
    public virtual AppUser? CreatedByNavigation { get; set; }
    public virtual AppUser? UpdatedByNavigation { get; set; }

}


