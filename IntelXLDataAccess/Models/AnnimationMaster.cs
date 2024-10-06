namespace IntelXLDataAccess.Models;

public partial class AnnimationMaster
{
    public int AnnimationId { get; set; }

    public string? AnnimationUrl { get; set; }

    public string VideoUrl { get; set; } = null!;

    public int QuestionId { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? CreatedDttm { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDttm { get; set; }

    public virtual AppUser? CreatedByNavigation { get; set; }
    public bool Status { get; set; } = true;
    public virtual AppUser? UpdatedByNavigation { get; set; }

    public virtual QuestionMaster Question { get; set; } = null!;
}
