using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class TopicMaster
{
    [JsonPropertyName("topicId")]
    public int TopicId { get; set; }

    [JsonPropertyName("topic")]
    public string Topic { get; set; } = null!;

    [JsonPropertyName("unitId")]
    public int UnitId { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [JsonPropertyName("subTopicMasters")]
    public virtual ICollection<SubTopicMaster> SubTopicMasters { get; set; } = new List<SubTopicMaster>();

    [JsonPropertyName("unit")]
    public virtual UnitMaster? Unit { get; set; }
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
