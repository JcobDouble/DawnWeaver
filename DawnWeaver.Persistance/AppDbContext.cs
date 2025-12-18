using System.Reflection;
using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDateTime dateTime) : DbContext(options), IAppDbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EventException> EventExceptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Seed();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach(var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = string.Empty;
                    entry.Entity.CreatedAt = dateTime.Now;
                    entry.Entity.UpdatedBy = string.Empty;
                    entry.Entity.UpdatedAt = dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = string.Empty;
                    entry.Entity.UpdatedAt = dateTime.Now;
                    break;
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}