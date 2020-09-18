namespace Application.Common.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface ITrackerDbContext
    {
        DbSet<Project> Projects { get; set; }

        DbSet<ProjectParticipant> ProjectParticipants { get; set; }

        DbSet<Issue> Issues { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}