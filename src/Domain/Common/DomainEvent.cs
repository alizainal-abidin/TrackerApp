namespace Domain.Common
{
    using System;

    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            this.DateOccurred = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}