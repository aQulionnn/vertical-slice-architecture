using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApi.Primitives;

namespace WebApi.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DbContext? context = eventData.Context; 
        
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        IEnumerable<EntityEntry<ISoftDeletableEntity>> entries = context.ChangeTracker
            .Entries<ISoftDeletableEntity>()
            .Where(x => x.State == EntityState.Deleted); 
        
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in entries)
        {
            entityEntry.State = EntityState.Modified;
            entityEntry.Entity.IsDeleted = true;
            entityEntry.Entity.DeletedOn = DateTime.Now;
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}