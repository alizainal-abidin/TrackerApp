namespace Infrastructure.Persistence.Seed
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;

    public class TrackerDbContextSeed
    {
        public static async Task SeedSampleDataAsync(TrackerDbContext context)
        {
            // seed predefined data.
            if (!context.Projects.Any())
            {
                // predefined project.
                var project = context.Projects.Add(new Project
                {
                    Key = "TRCK",
                    Name = "Tracker System",
                    OwnerId = "manager@terkwaz.com"
                });

                // add creator/owner as a default participant.
                context.ProjectParticipants.Add(new ProjectParticipant
                {
                    AddedBy = "manager@terkwaz.com",
                    ProjectId = project.Entity.Id,
                    UserId = "manager@terkwaz.com"
                });

                // add new issue.
                context.Issues.Add(new Issue
                {     
                    Id = $"{project.Entity.Key}-{1}",
                    ProjectId = project.Entity.Id,
                    Description = "User Story for Tracker Sysytem Project description",
                    Reporter = "manager@terkwaz.com",
                    Status = Domain.Enums.IssueStatus.Todo,
                    StoryPoint = 8,
                    Title = "User Story for Tracker Sysytem Project",
                    Type = Domain.Enums.IssueType.Story
                });

                await context.SaveChangesAsync();
            }
        }
    }
}