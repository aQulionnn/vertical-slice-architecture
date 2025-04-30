namespace WebApi.Common;

public static class Tenants
{
    public const int Tenant1 = 1;
    public const int Tenant2 = 2;
    public const int Tenant3 = 3;
    
    public static readonly int[] All = [Tenant1, Tenant2, Tenant3];
}