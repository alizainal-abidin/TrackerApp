namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using Domain.Common;
    using Domain.Events;

    public class Project : AuditableEntity
    {
        public Project()
        {
            this.Participants = new List<ProjectParticipant>();
            this.Issues = new List<Issue>();

            this.DomainEvents.Add(new ProjectCreatedEvent(this));
        }

        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public bool IsDeleted { get; set; }

        public IList<ProjectParticipant> Participants { get; private set; }

        public IList<Issue> Issues { get; private set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}