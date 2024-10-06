using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class UserSubscription
{
    [JsonPropertyName("userSubscriptionId")]
    public int UserSubscriptionId { get; set; }
    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }

    [JsonPropertyName("subscriptionId")]
    public int SubscriptionId { get; set; }

    [JsonPropertyName("subscriptionType")]
    public string? SubscriptionType { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;

    [JsonPropertyName("createdDttm")]
    public DateTime CreatedDttm { get; set; }

    [JsonPropertyName("expireDttm")]
    public DateTime ExpireDttm { get; set; }

    [JsonPropertyName("remaingDays")]
    public int RemaingDays { get; set; }

    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }

    [JsonPropertyName("subscription")]
    public virtual SubscriptionMaster? Subscription { get; set; } 
}

