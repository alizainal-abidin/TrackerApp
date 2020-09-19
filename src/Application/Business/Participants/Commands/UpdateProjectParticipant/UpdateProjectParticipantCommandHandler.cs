namespace Application.Business.Participants.Commands.UpdateProjectParticipant
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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

            var entry = this.context.ProjectParticipants.Attach(entity);
            entity.IsDeleted = request.IsDeleted;
            entry.State = EntityState.Modified;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}