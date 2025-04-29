using WebApi.Common;
using WebApi.Primitives;

namespace WebApi.Entities;

public class Museum : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime EstablishedAt { get; set; }
}