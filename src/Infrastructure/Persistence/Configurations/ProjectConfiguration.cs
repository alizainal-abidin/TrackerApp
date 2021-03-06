﻿namespace Infrastructure.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Ignore(p => p.DomainEvents);

            builder.HasKey(p => p.Id);

            builder.HasAlternateKey(p => p.Key);
            builder.Property(p => p.Key)
                .HasMaxLength(4);

            builder.Property(p => p.Name)
                .HasMaxLength(500);
        }
    }
}