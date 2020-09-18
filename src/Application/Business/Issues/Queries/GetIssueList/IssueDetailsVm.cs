namespace Application.Business.Issues.Queries.GetIssueList
{
    using System.Collections.Generic;
    using Application.Common.Helpers.Extensions;
    using Application.Common.Helpers.Mappings;
    using Application.Shared.Models;
    using AutoMapper;
    using Domain.Entities;
    using Domain.Enums;

    public class IssueDetailsVm : IMapFrom<Issue>
    {
        public string Id { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }

        public IssueDetailsVm Parent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, IssueDetailsVm>().IgnoreAllNonExisting();
            profile.CreateMap<IssueDetailsVm, Issue>().IgnoreAllNonExisting();
        }

        public class IssueListResult : CollectionResponse<IssueDetailsVm>
        {
            public IssueListResult(
                IReadOnlyCollection<IssueDetailsVm> items,
                int count,
                int pageIndex,
                int pageSize)
                : base(items, count, pageIndex, pageSize)
            {
            }
        }
    }
}