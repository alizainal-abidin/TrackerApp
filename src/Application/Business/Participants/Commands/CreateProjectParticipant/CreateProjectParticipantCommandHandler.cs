namespace Application.Business.Participants.Commands.CreateProjectParticipant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;

    public class CreateProjectParticipantCommandHandler : IRequestHandler<CreateProjectParticipantCommand, Guid>
    {
        private readonly ITrackerDbContext context;

        public CreateProjectParticipantCommandHandler(ITrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Handle(CreateProjectParticipantCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProjectParticipant
            {
                AddedBy = request.AddedBy,
                ProjectId = request.ProjectId,
                UserId = request.UserId
            };

            this.context.ProjectParticipants.Add(entity);

            await this.context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}