namespace Application.Business.Participants.Queries.GetParticipants
{
    using System.Collections.Generic;
    using Application.Shared.Models;

    public class ParticipantListResult : CollectionResponse<ParticipantVm>
    {
        public ParticipantListResult(
            IReadOnlyCollection<ParticipantVm> items,
            int count,
            int pageIndex,
            int pageSize)
            : base(items, count, pageIndex, pageSize)
        {
        }
    }
}