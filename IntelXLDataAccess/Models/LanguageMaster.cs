using System;
using System.Collections.Generic;

namespace IntelXLDataAccess.Models;

public partial class LanguageMaster
{
    public int LanguageId { get; set; }

    public string Language { get; set; } = null!;

    public bool Status { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? CreatedDttm { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDttm { get; set; }

    public virtual AppUser? CreatedByNavigation { get; set; }

    public virtual AppUser? UpdatedByNavigation { get; set; }
}
