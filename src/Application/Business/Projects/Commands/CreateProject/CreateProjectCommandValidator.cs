namespace Application.Business.Projects.Commands.CreateProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly ITrackerDbContext context;

        public CreateProjectCommandValidator(ITrackerDbContext context)
        {
            this.context = context;

            this.RuleFor(v => v.Name)
                .MaximumLength(500)
                .NotEmpty();

            this.RuleFor(v => v.OwnerId)
                .NotEmpty();

            // TODO: Move this validation logic.
            // Although this will work fine, 
            // it is not a best practice to check for database existence in this validation layer,            
            // instead, this validation should be done in the business layer.
            // Opinion needed.
            this.RuleFor(v => v.Key)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Key must not less than 3 characters.")
                .MaximumLength(4).WithMessage("Key must not exceed 4 characters.")
                .MustAsync(this.UniqueKey).WithMessage("The specified 'Key' is already exists.");
        }

        private async Task<bool> UniqueKey(string key, CancellationToken cancellationToken)
        {
            return await this.context.Projects.AllAsync(p => p.Key != key);
        }
    }
}