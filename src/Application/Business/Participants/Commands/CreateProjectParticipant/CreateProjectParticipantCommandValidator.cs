namespace Application.Business.Participants.Commands.CreateProjectParticipant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class CreateProjectParticipantCommandValidator : AbstractValidator<CreateProjectParticipantCommand>
    {
        private readonly ITrackerDbContext context;

        public CreateProjectParticipantCommandValidator(ITrackerDbContext context)
        {
            this.context = context;

            this.RuleFor(p => p.UserId)
                .NotEmpty();

            this.RuleFor(p => p.ProjectId)
                .NotEmpty();

            this.RuleFor(v => v)
                .MustAsync((obj, domain) => this.IsProductOwner(obj.AddedBy, obj.ProjectId, default))
                .WithMessage("Unable to add participant, either you have no authorization or the Project has been removed");

            this.RuleFor(p => p)
                .MustAsync((obj, domain) => this.NotExist(obj.ProjectId, obj.UserId, default))
                .WithMessage("Participant has already been added to this project");
        }

        private async Task<bool> NotExist(Guid projectId, string userId, CancellationToken cancellationToken)
        {
            var participant = await this.context.ProjectParticipants
                .AnyAsync(p => p.ProjectId == projectId && p.UserId == userId && !p.IsDeleted);

            return !participant;
        }

        private async Task<bool> IsProductOwner(string owner, Guid id, CancellationToken cancellationToken)
        {
            return await this.context.Projects.AllAsync(p => p.Id == id && p.OwnerId == owner && !p.IsDeleted);
        }
    }
}