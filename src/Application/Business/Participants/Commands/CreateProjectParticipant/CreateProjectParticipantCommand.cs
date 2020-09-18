namespace Application.Business.Participants.Commands.CreateProjectParticipant
{
    using System;
    using MediatR;

    public class CreateProjectParticipantCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }

        public string UserId { get; set; }

        public string AddedBy { get; set; }
    }
}