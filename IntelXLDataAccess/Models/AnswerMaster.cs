using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class AnswerMaster
{
    [JsonPropertyName("answerId")]
    public int AnswerId { get; set; }

    [JsonPropertyName("answer")]
    public string Answer { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
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

    [JsonPropertyName("questionMasters")]
    public virtual ICollection<QuestionMaster> QuestionMasters { get; set; } = new List<QuestionMaster>();
}
