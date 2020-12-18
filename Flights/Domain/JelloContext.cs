using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flights.Domain
{
    public partial class FlightsContext : DbContext
    {
        public FlightsContext() { }

        public FlightsContext(DbContextOptions<FlightsContext> options) : base(options) { }
        
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<IssueHistory> IssueHistory { get; set; }

        public virtual DbSet<Error> Error { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Viewed> Viewed { get; set; }

        public virtual DbSet<User> User { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUser = ServiceLocator.Resolve<ICurrentUser>();

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = entry.Entity.ChangedBy = currentUser.Id;
                        entry.Entity.CreatedOn = entry.Entity.ChangedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ChangedBy = currentUser.Id;
                        entry.Entity.ChangedOn = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
