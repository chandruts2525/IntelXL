using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class LanguageOfInstructionMaster
{
    [JsonPropertyName("languageId")]
    public int LanguageId { get; set; }
    [JsonPropertyName("language")]
    public string Language { get; set; } = null!;
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("order")]
    public int Order { get; set; } = 2;
    [JsonPropertyName("courseMasters")]
    public virtual ICollection<CourseMaster> CourseMasters { get; set; } = new List<CourseMaster>();
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
