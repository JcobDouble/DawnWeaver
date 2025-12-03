using DawnWeaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<EventType> EventTypes { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}