namespace Application.Business.Projects.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Shared.Models;
    using Application.Business.Participants.Commands.CreateProjectParticipant;
    using Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ProjectCreatedEventHandler : INotificationHandler<DomainEventNotification<ProjectCreatedEvent>>
    {
        private readonly ILogger<ProjectCreatedEventHandler> logger;
        private readonly IMediator mediator;

        public ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Handle(DomainEventNotification<ProjectCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            // add current user as a default participant here.
            await this.mediator.Send(new CreateProjectParticipantCommand
            {
                AddedBy = domainEvent.Project.OwnerId,
                ProjectId = domainEvent.Project.Id,
                UserId = domainEvent.Project.OwnerId,
            });

            this.logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return;
        }
    }
}