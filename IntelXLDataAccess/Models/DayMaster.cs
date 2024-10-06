using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class DayMaster
{
    [JsonPropertyName("dayId")]
    public int DayId { get; set; }
    [JsonPropertyName("dayName")]
    public string DayName { get; set; } = null!;
    [JsonPropertyName("tutorTimingConfigs")]
    public virtual ICollection<TutorTimingConfig> TutorTimingConfigs { get; set; } = new List<TutorTimingConfig>();
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
