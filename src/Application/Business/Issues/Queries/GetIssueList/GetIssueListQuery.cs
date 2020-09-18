namespace Application.Business.Issues.Queries.GetIssueList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common;
    using Application.Common.Helpers.Mappings;
    using Application.Common.Interfaces;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetIssueListQuery : IRequest<IssueDetailsVm.IssueListResult>
    {
        public Guid ProjectId { get; set; }

        public string Assignee { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public class GetIssuesQueryValidator : AbstractValidator<GetIssueListQuery>
        {
            public GetIssuesQueryValidator()
            {
                this.RuleFor(i => i.ProjectId).NotEmpty();
            }
        }

        public class GetIssuesQueryHandler : BaseHandler, IRequestHandler<GetIssueListQuery, IssueDetailsVm.IssueListResult>
        {
            public GetIssuesQueryHandler(ITrackerDbContext context, IMapper mapper)
                : base(context, mapper)
            {
            }

            public async Task<IssueDetailsVm.IssueListResult> Handle(GetIssueListQuery request, CancellationToken cancellationToken)
            {
                var vm = await this.Context.Issues
                    .AsNoTracking()
                    .Where(p => p.ProjectId == request.ProjectId && !p.IsDeleted)
                    .Include(i => i.Parent)
                    .ProjectTo<IssueDetailsVm>(this.Mapper.ConfigurationProvider)
                    .CollectionResponseAsync(request.PageNumber, request.PageSize);

                return new IssueDetailsVm.IssueListResult(vm.Items, vm.TotalCount, vm.PageIndex, vm.TotalPages);
            }
        }
    }
}