namespace Application.Business.Issues.Commands.CreateIssue
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, string>
    {
        private readonly ITrackerDbContext context;

        public CreateIssueCommandHandler(ITrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<string> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
        {
            var project = await this.context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var entity = new Issue
            {
                Description = request.Description,
                ProjectId = request.ProjectId,
                ParentId = request.ParentId,
                Reporter = request.Reporter,
                Status = Domain.Enums.IssueStatus.Todo,
                StepsToReplicate = request.StepsToReplicate,
                StoryPoint = request.StoryPoint,
                Title = request.Title,
                Type = request.Type,
            };

            // race condition handling
            var i = 0;
            do
            {
                // generate the unique issue id.
                var autoNumber = await this.context.Issues
                    .AsNoTracking()
                    .Where(i => i.ProjectId == request.ProjectId)
                    .CountAsync();

                entity.Id = $"{project.Key}-{autoNumber++}";

                // try to save here.
                this.context.Issues.Add(entity);
                var result = await this.context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                {
                    // on success results, return the entity.Id.
                    return entity.Id;
                }

                i++;
            }
            while (i < 10);

            throw new RaceConditionException(nameof(Issue), entity.Id);
        }
    }
}