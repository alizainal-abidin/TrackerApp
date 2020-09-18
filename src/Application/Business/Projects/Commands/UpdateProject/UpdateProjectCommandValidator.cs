namespace Application.Business.Projects.Commands.UpdateProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly ITrackerDbContext context;

        public UpdateProjectCommandValidator(ITrackerDbContext context)
        {
            this.context = context;

            this.RuleFor(v => v.Name)
                .MaximumLength(500)
                .NotEmpty();

            this.RuleFor(v => v.OwnerId)
                .NotEmpty();

            // TODO: Move this validation logic.
            // Although this will work fine, 
            // I don't think it is a good practice to check for database existence in this validation layer,            
            // instead, this validation should be done in the business layer.
            this.When(v => v.IsDeleted, () =>
            {
                this.RuleFor(v => v)
                    .MustAsync(this.IsTheOwner)
                    .OverridePropertyName("OwnerId")
                    .WithMessage("Unable to delete this Project! Only the owner can perform any delete operation.");
            });
        }

        public async Task<bool> IsTheOwner(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            // only owner can delete the project.
            return await this.context.Projects
                .AnyAsync(p => p.Id == request.Id && p.OwnerId == request.OwnerId);
        }
    }
}