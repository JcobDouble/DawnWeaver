using DawnWeaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Persistance;

public static class Seeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventType>(t =>
        {
            t.HasData(new EventType
            {
                Id = Guid.Parse("3c531b5d-4f0f-4e18-a08e-aae15ab4256a"),
                Name = "Work",
                Color = "#FF5733"
            }, new EventType
            {
                Id = Guid.Parse("d2fad8c4-7117-452c-868c-58b4dadd95ca"),
                Name = "Productivity",
                Color = "#33FF57"
            }, new EventType
            {
                Id = Guid.Parse("8d96ec16-572d-4124-88b2-2c9e9bb61027"),
                Name = "Free Time",
                Color = "#3357FF"
            });
        });
    }
}