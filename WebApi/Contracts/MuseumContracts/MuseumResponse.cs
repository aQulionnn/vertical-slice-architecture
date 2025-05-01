using WebApi.Common;

namespace WebApi.Contracts.MuseumContracts;

public class MuseumResponse : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime EstablishedAt { get; set; }
}