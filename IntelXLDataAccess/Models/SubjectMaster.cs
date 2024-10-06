using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class SubjectMaster
{
    [JsonPropertyName("subjectId")]
    public int SubjectId { get; set; }

    [JsonPropertyName("classId")]
    public int ClassId { get; set; }

    [JsonPropertyName("subjectName")]
    public string? SubjectName { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [NotMapped]
    [JsonPropertyName("subTopicCount")]
    public int subTopicCount { get; set; }

    [JsonPropertyName("class")]
    public virtual ClassMaster? Class { get; set; }
    [JsonPropertyName("createdBy")]
    public int? CreatedBy { get; set; }
    [JsonPropertyName("createdDttm")]
    public DateTime? CreatedDttm { get; set; }
    [JsonPropertyName("updatedBy")]
    public int? UpdatedBy { get; set; }
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }

    [JsonPropertyName("unitMasters")]
    public virtual ICollection<UnitMaster> UnitMasters { get; set; } = new List<UnitMaster>();
    [JsonPropertyName("createdByNavigation")]
    public virtual AppUser? CreatedByNavigation { get; set; }
    [JsonPropertyName("updatedByNavigation")]
    public virtual AppUser? UpdatedByNavigation { get; set; }
}
