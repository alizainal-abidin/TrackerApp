namespace Application.Business.Participants.Commands.UpdateProjectParticipant
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;

    public class UpdateProjectParticipantCommandHandler : IRequestHandler<UpdateProjectParticipantCommand>
    {
        private readonly ITrackerDbContext context;

        public UpdateProjectParticipantCommandHandler(ITrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateProjectParticipantCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.ProjectParticipants.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ProjectParticipant), request.Id);
            }

            entity.IsDeleted = request.IsDeleted;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}