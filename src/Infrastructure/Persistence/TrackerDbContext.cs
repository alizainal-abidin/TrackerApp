namespace Infrastructure.Persistence
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Domain.Common;
    using Domain.Entities;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class TrackerDbContext : DbContext, ITrackerDbContext
    {
        private readonly DbContextOptions options;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTimeService dateTimeService;
        private readonly IDomainEventService domainEventService;
        private readonly IOptions<OperationalStoreOptions> operationalStoreOptions;

        public TrackerDbContext(DbContextOptions options)
            : base(options)
        {
            this.options = options;
        }

            public TrackerDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            IDomainEventService domainEventService,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            this.options = options;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
            this.domainEventService = domainEventService;
            this.operationalStoreOptions = operationalStoreOptions;
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectParticipant> ProjectParticipants { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in this.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = this.currentUserService.UserId;
                        entry.Entity.CreatedDate = this.dateTimeService.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = this.currentUserService.UserId;
                        entry.Entity.LastModified = this.dateTimeService.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents(CancellationToken cancellationToken)
        {
            var domainEventEntities = this.ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .ToArray();

            foreach (var domainEvent in domainEventEntities)
            {
                await this.domainEventService.Publish(domainEvent);
            }
        }
    }
}