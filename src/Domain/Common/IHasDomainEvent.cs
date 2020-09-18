namespace Domain.Common
{
    using System.Collections.Generic;

    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}