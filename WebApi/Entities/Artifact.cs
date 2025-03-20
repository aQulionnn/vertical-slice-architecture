namespace WebApi.Entities;

public class Artifact
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public int EstimatedAge  { get; set; } 
    public string Location { get; set; } = string.Empty;
    public DateTime DiscoveryDate { get; set; } = DateTime.Now;
}