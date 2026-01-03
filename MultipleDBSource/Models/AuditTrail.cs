namespace MultipleDBSource.Models;

public class AuditTrail
{
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime ModifiedAt { get; set; }
}