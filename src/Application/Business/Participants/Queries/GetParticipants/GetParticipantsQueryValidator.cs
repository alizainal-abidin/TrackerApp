namespace Application.Business.Participants.Queries.GetParticipants
{
    using FluentValidation;

    public class GetParticipantsQueryValidator : AbstractValidator<GetParticipantsQuery>
    {
        public GetParticipantsQueryValidator()
        {
            this.RuleFor(v => v.ProjectId).NotNull().NotEmpty();
        }
    }
}