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
            
            var entry = this.Context.Issues.Attach(entity);                
                
            entity = this.Mapper.Map(request, entity);

            entry.State = EntityState.Modified;

            await this.Context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}