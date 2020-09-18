namespace Infrastructure.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.ToTable("ProjectIssues");

            builder.HasKey(p => p.Id);

            builder.Property(t => t.ProjectId)
                .IsRequired();

            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Type)
                .IsRequired();

            builder.Property(t => t.StoryPoint)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId);
        }
    }
}