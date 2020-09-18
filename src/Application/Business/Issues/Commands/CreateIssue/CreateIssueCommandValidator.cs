namespace Application.Business.Issues.Commands.CreateIssue
{
    using Domain.Enums;
    using FluentValidation;

    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator()
        {
            this.RuleFor(i => i.ProjectId).NotEmpty();

            this.RuleFor(i => i.Reporter).NotEmpty();

            this.RuleFor(i => i.Title).MaximumLength(200).NotEmpty();

            this.RuleFor(i => new { i.StoryPoint, i.StepsToReplicate })
                .Must(i => !i.StoryPoint.HasValue || i.StoryPoint == 0 ||
                    !string.IsNullOrEmpty(i.StepsToReplicate) || string.IsNullOrEmpty(i.StepsToReplicate))
                .WithMessage("StoryPoint and StepsToReplicate are mutually exclusive, an issue can not have both set.");

            this.When(i => i.Type == IssueType.Task, () => 
            {
                this.RuleFor(v => v.StepsToReplicate).Empty().WithMessage("a Task cannot has 'StepsToReplicate' property");
                this.RuleFor(v => v.StoryPoint).Empty().WithMessage("a Task cannot has 'StoryPoints' property");
            });

            this.When(i => i.Type == IssueType.Bug, () =>
            {                
                this.RuleFor(v => v.StoryPoint).Empty().WithMessage("a Bug cannot has 'StoryPoints' property");
            });

            this.When(i => i.Type == IssueType.Story, () =>
            {
                this.RuleFor(v => v.StepsToReplicate).Empty().WithMessage("a Story cannot has 'StoryPoints' property");
            });
        }
    }
}