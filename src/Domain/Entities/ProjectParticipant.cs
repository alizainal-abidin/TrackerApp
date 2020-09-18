namespace Domain.Entities
{
    using System;
    using Domain.Common;

    public class ProjectParticipant : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string UserId { get; set; }

        public string AddedBy { get; set; }

        public bool IsDeleted { get; set; }

        public Project Project { get; set; }
    }
}