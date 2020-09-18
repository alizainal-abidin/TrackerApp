namespace Application.Business.Projects.Commands.UpdateProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly ITrackerDbContext context;

        public UpdateProjectCommandHandler(ITrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Projects.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            entity.IsDeleted = request.IsDeleted;
            entity.Name = request.Name;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}