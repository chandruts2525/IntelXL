using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class TutorDetail
{
    [JsonPropertyName("detailId")]
    public int DetailId { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = null!;
    [JsonPropertyName("shortBio")]
    public string ShortBio { get; set; } = null!;
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("profileUrl")]
    public string ProfileUrl { get; set; } = null!;

    [JsonPropertyName("languagesSpeak")]
    public string LanguagesSpeak { get; set; } = null!;

    [JsonPropertyName("videoUrl")]
    public string VideoUrl { get; set; } = null!;    

    [JsonPropertyName("pricing")]
    public double Pricing { get; set; }

    [JsonPropertyName("timeZone")]
    public DateTimeOffset TimeZone { get; set; }
    [JsonPropertyName("tutorId")]
    public int TutorId { get; set; }
    [JsonPropertyName("courseId")]
    public int? CourseId { get; set; }
    [JsonPropertyName("classId")]
    public int? ClassId { get; set; }
    [JsonPropertyName("subjectId")]
    public int? SubjectId { get; set; }
    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }
    public DateTime? CreatedDttm { get; set; }
    public DateTime? UpdatedDttm { get; set; }
}
