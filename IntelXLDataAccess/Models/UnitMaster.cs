using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class UnitMaster
{
    [JsonPropertyName("unitId")]
    public int UnitId { get; set; }

    [JsonPropertyName("unitName")]
    public string? UnitName { get; set; }

    [JsonPropertyName("subjectid")]
    public int Subjectid { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [JsonPropertyName("subject")]
    public virtual SubjectMaster? Subject { get; set; }

    [JsonPropertyName("topicMasters")]
    public virtual ICollection<TopicMaster> TopicMasters { get; set; } = new List<TopicMaster>();
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
