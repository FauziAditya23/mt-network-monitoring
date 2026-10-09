namespace mt_network_monitoring.Models;

public class RouterStatus
{
    public int HistoryID { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public int RouterID { get; set; }
    public decimal? CPUUsage { get; set; }
    public decimal? MemoryUsage { get; set; }
    public string? Uptime { get; set; }
    public DateTime RecordedDate { get; set; }
    public string? Status { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
