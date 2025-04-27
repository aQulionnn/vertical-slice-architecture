using WebApi.Primitives;

namespace WebApi.Entities;

public class Artifact : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public int EstimatedAge  { get; set; } 
    public string Location { get; set; } = string.Empty;
    public DateTime DiscoveryDate { get; set; } = DateTime.Now;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}