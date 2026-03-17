namespace POSitive.API.Models.Dispensers;

public class DispenserStatusDto
{
    public int CompanyID { get; set; }
    public int LocationID { get; set; }
    public int Dispenser { get; set; }
    public string? DispenserStatus { get; set; }
    public int TimeSinceUpdateMinutes { get; set; }
    public DateTime LastUpdatedUtc { get; set; }
}
