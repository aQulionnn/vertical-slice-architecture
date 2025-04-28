using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Primitives;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Artifact> Artifacts { get; set; }
    public DbSet<Museum> Museums { get; set; }
    public DbSet<HistoricalEvent> HistoricalEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDeletableEntity.IsDeleted));
                var constant = Expression.Constant(false);
                var lambda = Expression.Lambda(Expression.Equal(property, constant), parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}