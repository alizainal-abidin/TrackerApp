namespace Application.Business.Projects.Queries.GetProjects
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Helpers.Mappings;
    using Application.Common.Interfaces;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, ProjectListResult>
    {
        private readonly ITrackerDbContext context;
        private readonly IMapper mapper;

        public GetProjectsQueryHandler(ITrackerDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ProjectListResult> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var entities = this.context.Projects
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            // filter.
            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                entities = entities.Where(p => p.OwnerId.ToLower() == request.OwnerId.ToLower());
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                entities = entities.Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));
            }

            var vm = await entities.OrderBy(p => p.Name)
                .ProjectTo<ProjectVm>(this.mapper.ConfigurationProvider)
                .CollectionResponseAsync(request.PageNumber, request.PageSize);

            return new ProjectListResult(vm.Items, vm.TotalCount, vm.PageIndex, vm.TotalPages);
        }
    }
}