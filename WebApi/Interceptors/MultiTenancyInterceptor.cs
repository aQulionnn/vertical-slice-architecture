using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApi.Primitives;
using WebApi.Services;

namespace WebApi.Interceptors;

public class MultiTenancyInterceptor(TenantProvider tenantProvider) : SaveChangesInterceptor
{
    private readonly TenantProvider _tenantProvider = tenantProvider;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DbContext? context = eventData.Context;
        
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        int tenantId = _tenantProvider.GetTenantId();
        IEnumerable<EntityEntry<IMultiTenant>> entries = context.ChangeTracker.Entries<IMultiTenant>();

        foreach (EntityEntry<IMultiTenant> entityEntry in entries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    entityEntry.Property(x => x.TenantId).CurrentValue = tenantId;
                    break;
                
                case EntityState.Modified:
                    entityEntry.Property(x => x.TenantId).IsModified = false;
                    break;
                
                case EntityState.Deleted:
                    if (entityEntry.Entity.TenantId != tenantId)
                        throw new UnauthorizedAccessException("Cannot delete data belonging to another tenant");
                    break;
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}