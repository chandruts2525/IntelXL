using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class TimingMaster
{
    [JsonPropertyName("timingId")]
    public int TimingId { get; set; }
    [JsonPropertyName("timing")]
    public string Timing { get; set; } = null!;
    [JsonPropertyName("studentTutorScheduleFromTimes")]
    public virtual ICollection<StudentTutorSchedule> StudentTutorScheduleFromTimes { get; set; } = new List<StudentTutorSchedule>();
    [JsonPropertyName("studentTutorScheduleToTimes")]
    public virtual ICollection<StudentTutorSchedule> StudentTutorScheduleToTimes { get; set; } = new List<StudentTutorSchedule>();
    [JsonPropertyName("tutorTimingConfigFromTimes")]
    public virtual ICollection<TutorTimingConfig> TutorTimingConfigFromTimes { get; set; } = new List<TutorTimingConfig>();
    [JsonPropertyName("tutorTimingConfigToTimes")]
    public virtual ICollection<TutorTimingConfig> TutorTimingConfigToTimes { get; set; } = new List<TutorTimingConfig>();
    public int? CreatedBy { get; set; }

    public DateTime? CreatedDttm { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDttm { get; set; }
    public virtual AppUser? CreatedByNavigation { get; set; }
    public virtual AppUser? UpdatedByNavigation { get; set; }
}
