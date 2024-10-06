using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class CourseMaster
{
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }
    [JsonPropertyName("courseName")]
    public string? CourseName { get; set; }
    [JsonPropertyName("courseDuration")]
    public decimal? CourseDuration { get; set; }
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("languageId")]
    public int LanguageId { get; set; }
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [JsonPropertyName("classMasters")]
    public virtual ICollection<ClassMaster> ClassMasters { get; set; } = new List<ClassMaster>();
    [JsonPropertyName("LanguageOfInstruction")]
    public virtual LanguageOfInstructionMaster? LanguageOfInstruction { get; set; }
    [JsonPropertyName("subscriptionMasters")]
    public virtual ICollection<SubscriptionMaster> SubscriptionMasters { get; set; } = new List<SubscriptionMaster>();
    [JsonPropertyName("createdBy")]
    public int? CreatedBy { get; set; }
    [JsonPropertyName("createdDttm")]
    public DateTime? CreatedDttm { get; set; }
    [JsonPropertyName("updatedBy")]
    public int? UpdatedBy { get; set; }
    [JsonPropertyName("createdByNavigation")]
    public virtual AppUser? CreatedByNavigation { get; set; }
    [JsonPropertyName("updatedByNavigation")]
    public virtual AppUser? UpdatedByNavigation { get; set; }
}
