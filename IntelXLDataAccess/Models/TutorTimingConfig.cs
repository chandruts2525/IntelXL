using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class TutorTimingConfig
{
    [JsonPropertyName("tutorTimingId")]
    public int TutorTimingId { get; set; }
    [JsonPropertyName("fromTimeId")]
    public int FromTimeId { get; set; }
    [JsonPropertyName("toTimeId")]
    public int ToTimeId { get; set; }
    [JsonPropertyName("tutorId")]
    public int TutorId { get; set; }
    [JsonPropertyName("dayId")]
    public int DayId { get; set; }
    [JsonPropertyName("day")]
    public virtual DayMaster? Day { get; set; }
    [JsonPropertyName("fromTime")]
    public virtual TimingMaster? FromTime { get; set; }
    [JsonPropertyName("toTime")]
    public virtual TimingMaster? ToTime { get; set; }
    [JsonPropertyName("tutor")]
    public virtual AppUser? Tutor { get; set; }
    [JsonPropertyName("studentTutorSchedules")]
    public virtual ICollection<StudentTutorSchedule> StudentTutorSchedules{ get; set; } = new List<StudentTutorSchedule>();
   
    public DateTime? CreatedDttm { get; set; }
    
    public DateTime? UpdatedDttm { get; set; }
    
}
