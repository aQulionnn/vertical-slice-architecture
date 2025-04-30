namespace WebApi.Primitives;

public interface IMultiTenant
{
    public int TenantId { get; set; }
}