namespace Tracker.App.Infrastructure.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.Shared.Models;
    using Domain.Common;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> logger;
        private readonly IMediator mediator;

        public DomainEventService(ILogger<DomainEventService> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            this.logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
            await this.mediator.Publish(this.GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}