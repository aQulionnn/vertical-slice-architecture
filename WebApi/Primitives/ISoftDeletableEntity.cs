namespace WebApi.Primitives;

public interface ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}