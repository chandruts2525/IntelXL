using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class SubTopicMaster
{
    [JsonPropertyName("subTopicId")]
    public int SubTopicId { get; set; }
    [JsonPropertyName("subTopic")]
    public string SubTopic { get; set; } = null!;
    [JsonPropertyName("topicId")]
    public int TopicId { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [JsonPropertyName("questionMasters")]
    public virtual ICollection<QuestionMaster> QuestionMasters { get; set; } = new List<QuestionMaster>();
    [JsonPropertyName("topic")]
    public virtual TopicMaster? Topic { get; set; }
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
