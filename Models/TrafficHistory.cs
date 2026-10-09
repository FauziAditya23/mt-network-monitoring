namespace mt_network_monitoring.Models;

public class TrafficHistory
{
    public int TrafficID { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public int RouterID { get; set; }
    public int InterfaceID { get; set; }
    public decimal? RXRate { get; set; }
    public decimal? TXRate { get; set; }
    public DateTime RecordedDate { get; set; }
    public string? Status { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
