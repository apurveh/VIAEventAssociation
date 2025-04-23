using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Values;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            // Key
            builder.Property(e => e.Id)
                   .HasConversion(
                          id => id.Value,
                          value => EventId.FromString(value)
                   )
                   .ValueGeneratedNever();

            // Title
            builder.Property(e => e.EventTitle)
                   .HasConversion(v => v.Value, v => EventTitle.Create(v).Payload)
                   .HasMaxLength(75)
                   .IsRequired();

            // Description
            builder.Property(e => e.EventDescription)
                   .HasConversion(v => v.Value, v => EventDescription.Create(v).Payload)
                   .HasMaxLength(250);

            // Visibility
            builder.Property(e => e.EventVisibility)
                   .HasConversion<string>()
                   .IsRequired();

            // Status
            builder.Property(e => e.EventStatus)
                   .HasConversion<string>()
                   .IsRequired();

            // Max Guests
            builder.Property(e => e.MaxNumberOfGuests)
                   .HasConversion(v => v.Value, v => NumberOfGuests.Create(v).Payload)
                   .IsRequired();

            // EventTime (start + end)
            builder.OwnsOne(e => e.EventTime, eb =>
            {
                eb.Property(et => et.Start)
                  .HasColumnName("StartTime")
                  .IsRequired();

                eb.Property(et => et.End)
                  .HasColumnName("EndTime")
                  .IsRequired();
            });

            // Participations (HashSet<Participation>)
            builder.HasMany<Participation>(e => e.Participations)
                   .WithOne()
                   .HasForeignKey("EventId")
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: ignore ConfirmedParticipants if you don't want EF trying to persist it
            builder.Ignore(e => e.ConfirmedParticipants);
        }
    }
}
