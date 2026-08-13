namespace SmartCityComplaint.Models;

public class Complaint
{
    public int Id { get; set; }

    public string Description { get; set; } = "";

    public string Location { get; set; } = "";

    public string Category { get; set; } = "";

    public string Priority { get; set; } = "";

    public string Status { get; set; } = "Pending";

    public string Department { get; set; } = "";

    public int UserId { get; set; }

    public string AIAnalysis { get; set; } = "";
}