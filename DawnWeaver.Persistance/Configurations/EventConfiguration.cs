using DawnWeaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DawnWeaver.Persistance.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e => e.Description)
            .HasMaxLength(500);
        
        builder.Property(e => e.StartDate)
            .HasDefaultValue(DateTime.Now)
            .IsRequired();
        
        builder.Property(e => e.EndDate)
            .HasDefaultValue(DateTime.Now.AddHours(1))
            .IsRequired();

        builder.Property(e => e.DurationInMinutes)
            .IsRequired()
            .HasDefaultValue(60);

        builder.HasMany(e => e.EventExceptions)
            .WithOne(e => e.Event);
    }
}