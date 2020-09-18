namespace Application.Business.Participants.Queries.GetParticipants
{
    using System;
    using Application.Common.Helpers.Extensions;
    using Application.Common.Helpers.Mappings;
    using AutoMapper;
    using Domain.Entities;

    public class ParticipantVm : IMapFrom<ProjectParticipant>
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string UserId { get; set; }

        public string AddedBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProjectParticipant, ParticipantVm>().IgnoreAllNonExisting();
            profile.CreateMap<ParticipantVm, ProjectParticipant>().IgnoreAllNonExisting();
        }
    }
}