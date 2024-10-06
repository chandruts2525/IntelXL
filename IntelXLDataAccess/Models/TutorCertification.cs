using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class TutorCertification
{
    [JsonPropertyName("certificateId")]
    public int CertificateId { get; set; }
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = null!;
    [JsonPropertyName("certificateName")]
    public string CertificateName { get; set; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [JsonPropertyName("issuedBy")]
    public string IssuedBy { get; set; } = null!;
    [JsonPropertyName("yearsOfStudy")]
    public string YearsOfStudy { get; set; } = null!;
    [JsonPropertyName("certificateUrl")]
    public string CertificateUrl { get; set; } = null!;
    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }
    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }

    public DateTime? CreatedDttm { get; set; }

    public DateTime? UpdatedDttm { get; set; }
}
