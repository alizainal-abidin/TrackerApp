namespace Application.Business.Participants.Queries.GetParticipants
{
    using System;
    using MediatR;

    public class GetParticipantsQuery : IRequest<ParticipantListResult>
    {
        public Guid ProjectId { get; set; }

        public string ParticipantId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}