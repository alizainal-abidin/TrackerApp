namespace Application.Shared.Models
{
    using Domain.Common;
    using MediatR;

    public class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            this.DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}