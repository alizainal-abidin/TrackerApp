namespace Infrastructure.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ParticipantConfiguration : IEntityTypeConfiguration<ProjectParticipant>
    {
        public void Configure(EntityTypeBuilder<ProjectParticipant> builder)
        {
            builder.ToTable("ProjectParticipants");

            builder.HasKey(p => p.Id);

            builder.Property(t => t.ProjectId)
                .IsRequired();

            builder.HasOne(d => d.Project)
                .WithMany(p => p.Participants)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}