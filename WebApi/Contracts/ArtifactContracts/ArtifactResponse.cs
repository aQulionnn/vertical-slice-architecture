namespace WebApi.Contracts.ArtifactContracts;

public class ArtifactResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Origin { get; set; } 
    public string Material { get; set; }
    public int EstimatedAge  { get; set; } 
    public string Location { get; set; } 
    public DateTime DiscoveryDate { get; set; } 
}