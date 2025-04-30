using WebApi.Primitives;

namespace WebApi.Common;

public abstract class BaseEntity : IEntity, IAuditableEntity, ISoftDeletableEntity, IMultiTenant
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int TenantId { get; set; }
}

public interface IEntity 
{
    Guid Id { get; set; }
}