using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class ChoiceMaster
{
    [JsonPropertyName("choiceId")]
    public int ChoiceId { get; set; }

    [JsonPropertyName("choice")]
    public string? Choice { get; set; }
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;

    [JsonPropertyName("questionId")]
    public int QuestionId { get; set; }
    [JsonPropertyName("question")]
    public virtual QuestionMaster? Question { get; set; }
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
