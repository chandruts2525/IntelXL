using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class QuestionMaster
{
    [JsonPropertyName("questionId")]
    public int QuestionId { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; } = null!;

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;

    [JsonPropertyName("subTopicId")]
    public int SubTopicId { get; set; }

    [JsonPropertyName("answerId")]
    public int? AnswerId { get; set; }
    [JsonPropertyName("isVerified")]
    public bool IsVerified { get; set; } = false;
    [JsonPropertyName("questionType")]
    public int QuestionType { get; set; } = 1;

    [JsonPropertyName("isPreviousYearQuestion")]
    public bool IsPreviousYearQuestion { get; set; } = false;
    [JsonPropertyName("isProbable")]
    public bool IsProbable { get; set; } = false;

    [JsonPropertyName("previousYear")]
    public string? PreviousYear { get; set; } 
    [JsonPropertyName("probableYear")]
    public string? ProbableYear { get; set; }
    public virtual ICollection<AnnimationMaster> AnnimationMasters { get; set; } = new List<AnnimationMaster>();
    [JsonPropertyName("answer")]
    public virtual AnswerMaster? Answer { get; set; }

    [JsonPropertyName("choiceMasters")]
    public virtual ICollection<ChoiceMaster> ChoiceMasters { get; set; } = new List<ChoiceMaster>();

    [JsonPropertyName("subTopic")]
    public virtual SubTopicMaster? SubTopic { get; set; }
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
public class ArrangedQuestions
{
    public List<QuestionMaster>? Questions { get; set; }
    public int TotalCount { get; set; }
    public int TrialCount { get; set; }
}
