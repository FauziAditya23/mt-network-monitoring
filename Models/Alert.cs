namespace mt_network_monitoring.Models;

public class Alert
{
    public int AlertID { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public int RouterID { get; set; }
    public int? InterfaceID { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public string? Message { get; set; }
    public decimal? Value { get; set; }
    public decimal? Threshold { get; set; }
    public DateTime AlertDate { get; set; }
    public string? Status { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
