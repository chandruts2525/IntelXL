using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class StudentTutorSchedule
{
    [JsonPropertyName("scheduleId")]
    public int ScheduleId { get; set; }

    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }

    [JsonPropertyName("tutorId")]
    public int TutorId { get; set; }

    [JsonPropertyName("tutorTimingConfigId")]
    public int TutorTimingConfigId { get; set; }

    [JsonPropertyName("fromTimeId")]
    public int FromTimeId { get; set; }

    [JsonPropertyName("toTimeId")]
    public int ToTimeId { get; set; }

    [JsonPropertyName("scheduledDate")]
    public DateTime ScheduledDate { get; set; }

    [JsonPropertyName("createdDttm")]
    public DateTime CreatedDttm { get; set; }

    [JsonPropertyName("isPaid")]
    public bool? IsPaid { get; set; }

    [JsonPropertyName("status")]
    public bool? Status { get; set; }

    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }
    [JsonPropertyName("fromTime")]
    public virtual TimingMaster? FromTime { get; set; }
    [JsonPropertyName("toTime")]
    public virtual TimingMaster? ToTime { get; set; }
    [JsonPropertyName("tutor")]
    public virtual AppUser? Tutor { get; set; }
    
    [JsonPropertyName("tutorTimingConfig")]
    public virtual TutorTimingConfig? TutorTimingConfig { get; set; }
    [JsonPropertyName("updatedBy")]
    public int? UpdatedBy { get; set; }
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }
}
