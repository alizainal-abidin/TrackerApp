namespace Application.Business.Participants.Commands.UpdateProjectParticipant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class UpdateProjectParticipantCommandValidator : AbstractValidator<UpdateProjectParticipantCommand>
    {
        private readonly ITrackerDbContext context;

        public UpdateProjectParticipantCommandValidator(ITrackerDbContext context)
        {
            this.context = context;

            this.RuleFor(p => p.Id).NotEmpty();

            this.RuleFor(p => p.UserId).NotEmpty();

            this.RuleFor(v => v)
                .MustAsync((obj, domain) => this.IsOwner(obj.Id, obj.UserId, default))
                .WithMessage("Unable to update the participant, either you are not authorized or the Project has been removed");
        }

        private async Task<bool> IsOwner(Guid id, string userId, CancellationToken cancellationToken)
        {
            return await this.context.ProjectParticipants.AnyAsync(p => p.Id == id && p.AddedBy == userId);
        }
    }
}