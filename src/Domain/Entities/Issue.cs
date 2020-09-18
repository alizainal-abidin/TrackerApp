namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using Domain.Common;
    using Domain.Enums;

    public class Issue : AuditableEntity
    {
        public string Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Reporter { get; set; }

        public string Assignee { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }

        public string ParentId { get; set; }

        public decimal? StoryPoint { get; set; }

        public string StepsToReplicate { get; set; }

        public bool IsDeleted { get; set; }

        public Project Project { get; set; }

        public Issue Parent { get; set; }

        public virtual HashSet<Issue> Children { get; set; }
    }
}