
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Values;


namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Configuration
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guests");

            // Primary Key
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id)
                .HasConversion(id => id.Value, value => GuestId.FromString(value))
                .ValueGeneratedNever();

            // FirstName (NameType value object)
            builder.Property(g => g.FirstName)
                .HasConversion(name => name.Value, value => NameType.Create(value).Payload)
                .HasMaxLength(25)
                .IsRequired();

            // LastName (NameType value object)
            builder.Property(g => g.LastName)
                .HasConversion(name => name.Value, value => NameType.Create(value).Payload)
                .HasMaxLength(25)
                .IsRequired();

            // Email (value object)
            builder.Property(g => g.Email)
                .HasConversion(email => email.Value, value => Email.Create(value).Payload)
                .HasMaxLength(100)
                .IsRequired();

            // Participations (collection)
            builder.HasMany(g => g.Participations)
                .WithOne()
                .HasForeignKey("GuestId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
