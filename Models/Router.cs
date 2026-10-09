namespace mt_network_monitoring.Models;

public class Router
{
    public int RouterID { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string RouterName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string? RouterOSVersion { get; set; }
    public string? Uptime { get; set; }
    public string? Status { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
