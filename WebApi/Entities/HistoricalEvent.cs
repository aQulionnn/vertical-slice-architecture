using WebApi.Common;
using WebApi.Primitives;

namespace WebApi.Entities;

public class HistoricalEvent : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
}
