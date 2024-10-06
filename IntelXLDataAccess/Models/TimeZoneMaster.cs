using System;
using System.Collections.Generic;

namespace IntelXLDataAccess.Models;

public partial class TimeZoneMaster
{
    public int TimezoneId { get; set; }

    public string? TimezoneName { get; set; }

    public string? UtcOffSet { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? CreatedDttm { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDttm { get; set; }
    public virtual AppUser? CreatedByNavigation { get; set; }
    public virtual AppUser? UpdatedByNavigation { get; set; }
}
