namespace Application.Business.Projects.Commands.CreateProject
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using Domain.Events;
    using MediatR;

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly ITrackerDbContext context;

        public CreateProjectCommandHandler(ITrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new Project
            {
                Key = request.Key,
                Name = request.Name,
                OwnerId = request.OwnerId,
            };                      

            // HACK: add the owner as a default participants.
            // it is not ideal here,
            // just trying to demonstrate how we can create an Event and utilize Mediatr.INotification to handle it.
            // more common case: on creating a Project, system will send a notification email to the Project Owner.
            // entity.DomainEvents.Add(new ProjectCreatedEvent(entity));

            this.context.Projects.Add(entity);
            await this.context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}