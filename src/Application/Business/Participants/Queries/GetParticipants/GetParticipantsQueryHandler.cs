namespace Application.Business.Participants.Queries.GetParticipants
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

    public class GetParticipantsQueryHandler : IRequestHandler<GetParticipantsQuery, ParticipantListResult>
    {
        private readonly ITrackerDbContext context;
        private readonly IMapper mapper;

        public GetParticipantsQueryHandler(ITrackerDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ParticipantListResult> Handle(GetParticipantsQuery request, CancellationToken cancellationToken)
        {
            var entities = this.context.ProjectParticipants
                .AsNoTracking()
                .Where(p => p.ProjectId == request.ProjectId && !p.IsDeleted);
            
            if (!string.IsNullOrEmpty(request.ParticipantId))
            {
                entities = entities.Where(p => p.UserId == request.ParticipantId);
            }

            var vm = await entities
                .ProjectTo<ParticipantVm>(this.mapper.ConfigurationProvider)
                .CollectionResponseAsync(request.PageNumber, request.PageSize);

            return new ParticipantListResult(vm.Items, vm.TotalCount, vm.PageIndex, vm.TotalPages);
        }
    }
}