namespace Application.Business.Participants.Commands.UpdateProjectParticipant
{
    using System;
    using MediatR;

    public class UpdateProjectParticipantCommand : IRequest
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}