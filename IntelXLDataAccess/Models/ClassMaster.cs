using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class ClassMaster
{
    [JsonPropertyName("classId")]
    public int ClassId { get; set; }

    [JsonPropertyName("className")]
    public string ClassName { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [NotMapped]
    [JsonPropertyName("totalQuestionsCount")]
    public int TotalQuestionsCount { get; set; }

    [JsonPropertyName("course")]
    public virtual CourseMaster? Course { get; set; }

    [JsonPropertyName("subjectMasters")]
    public virtual ICollection<SubjectMaster> SubjectMasters { get; set; } = new List<SubjectMaster>();
    [JsonPropertyName("subscriptionMasters")]
    public virtual ICollection<SubscriptionMaster> SubscriptionMasters { get; set; } = new List<SubscriptionMaster>(); 
    [JsonPropertyName("createdBy")]
    public int? CreatedBy { get; set; }
    [JsonPropertyName("createdDttm")]
    public DateTime? CreatedDttm { get; set; }
    [JsonPropertyName("updatedBy")]
    public int? UpdatedBy { get; set; }
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }
    [JsonPropertyName("createdByNavigation")]
    public virtual AppUser? CreatedByNavigation { get; set; }
    [JsonPropertyName("updatedByNavigation")]
    public virtual AppUser? UpdatedByNavigation { get; set; }
}
