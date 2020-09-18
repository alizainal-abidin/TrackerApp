namespace Infrastructure.Persistence.Seed
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;

    public class TrackerDbContextSeed
    {
        public static async Task SeedSampleDataAsync(TrackerDbContext context, string userId)
        {
            // seed predefined data.
            if (!context.Projects.Any())
            {
                var a = context.Projects.Add(new Project
                {
                    Key = "TKWZ",
                    Name = "Tracker System",
                    OwnerId = userId
                });

                await context.SaveChangesAsync();

                context.ProjectParticipants.Add(new ProjectParticipant
                {
                    AddedBy = userId,
                    ProjectId = a.Entity.Id,
                    UserId = userId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}