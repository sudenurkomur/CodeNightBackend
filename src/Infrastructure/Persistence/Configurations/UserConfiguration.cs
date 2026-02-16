using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(u => u.Surname)
            .HasColumnName("surname")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();
    }
}
