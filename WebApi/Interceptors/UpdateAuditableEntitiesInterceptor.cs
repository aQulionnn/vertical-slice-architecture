using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApi.Primitives;

namespace WebApi.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DbContext? context = eventData.Context; 
        
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<IAuditableEntity>> entries = context.ChangeTracker.Entries<IAuditableEntity>();
        
        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property(x => x.CreatedOn).CurrentValue = DateTime.Now;
            
            if (entityEntry.State == EntityState.Modified)
                entityEntry.Property(x => x.ModifiedOn).CurrentValue = DateTime.Now;
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);   
    }
}