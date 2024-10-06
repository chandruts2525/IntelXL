using System;
using System.Collections.Generic;

namespace IntelXLDataAccess.Models;

public partial class TutorEducation
{
    public int EducationId { get; set; }

    public string University { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string YearsOfStudy { get; set; } = null!;

    public string CertificateUrl { get; set; } = null!;

    public int AppUserId { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;
    public DateTime? CreatedDttm { get; set; }
    public DateTime? UpdatedDttm { get; set; }
   
}
