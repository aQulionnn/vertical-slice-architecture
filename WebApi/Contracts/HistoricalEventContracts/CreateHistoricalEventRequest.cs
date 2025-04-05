namespace WebApi.Contracts.HistoricalEventContracts;

public class CreateHistoricalEventRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
}