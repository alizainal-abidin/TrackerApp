namespace Application.Business.Issues.Commands.UpdateIssue
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class UpdateIssueCommandHandler : BaseHandler, IRequestHandler<UpdateIssueCommand>
    {
        public UpdateIssueCommandHandler(ITrackerDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<Unit> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.Context.Issues.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Issue), request.Id);
            }

            entity = this.Mapper.Map<Issue>(request);
            await this.Context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}