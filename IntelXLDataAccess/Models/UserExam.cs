using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class UserExam
{
    [JsonPropertyName("userExamId")]
    public int UserExamId { get; set; }
    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }
    [JsonPropertyName("questionId")]
    public int QuestionId { get; set; }
    [JsonPropertyName("subtopicId")]
    public int SubtopicId { get; set; }
    [JsonPropertyName("subjectId")]
    public int SubjectId { get; set; }
    [JsonPropertyName("practiceType")]
    public string PracticeType { get; set; } = null!;
    [JsonPropertyName("answeredStatus")]
    public int AnsweredStatus { get; set; }
    public DateTime? CreatedDttm { get; set; }
    public DateTime? UpdatedDttm { get; set; }
    public string? YearOfQuestion { get; set; }
}
