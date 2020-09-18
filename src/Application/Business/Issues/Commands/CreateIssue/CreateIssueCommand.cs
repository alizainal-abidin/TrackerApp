namespace Application.Business.Issues.Commands.CreateIssue
{
    using System;
    using Domain.Enums;
    using MediatR;

    public class CreateIssueCommand : IRequest<string>
    {
        public Guid ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Reporter { get; set; }

        public string Assignee { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }

        public string ParentId { get; set; }

        public decimal? StoryPoint { get; set; }

        public string StepsToReplicate { get; set; }
    }
}