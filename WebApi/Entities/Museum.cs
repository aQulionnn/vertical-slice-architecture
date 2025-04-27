using WebApi.Primitives;

namespace WebApi.Entities;

public class Museum : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime EstablishedAt { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}