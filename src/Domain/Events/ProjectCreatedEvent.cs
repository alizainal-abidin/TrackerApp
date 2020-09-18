namespace Domain.Events
{
    using Domain.Common;
    using Domain.Entities;

    public class ProjectCreatedEvent : DomainEvent
    {
        public ProjectCreatedEvent(Project project)
        {
            this.Project = project;
        }

        public Project Project { get; }
    }
}