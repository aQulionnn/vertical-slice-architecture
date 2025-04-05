namespace WebApi.Entities;

public class HistoricalEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
}
