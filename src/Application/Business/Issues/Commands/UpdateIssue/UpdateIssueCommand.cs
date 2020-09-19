namespace Application.Business.Issues.Commands.UpdateIssue
{
    using Application.Common.Helpers.Extensions;
    using Application.Common.Helpers.Mappings;
    using AutoMapper;
    using Domain.Entities;
    using Domain.Enums;
    using MediatR;

    public class UpdateIssueCommand : IRequest, IMapFrom<Issue>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }

        public string ParentId { get; set; }

        public decimal? StoryPoint { get; set; }

        public string StepsToReplicate { get; set; }

        public bool IsDeleted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateIssueCommand, Issue>();
        }
    }
}