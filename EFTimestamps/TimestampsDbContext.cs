using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EFTimestamps.Extensions;
using EFTimestamps.Interfaces;

namespace EFTimestamps
{
    public class TimestampsDbContext : DbContext
    {
        #region Public constructors
        public TimestampsDbContext() : base() { }

        public TimestampsDbContext(string connectionString) : base(connectionString) { }

        public TimestampsDbContext(DbCompiledModel model) : base(model) { }

        public TimestampsDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection) { }

        public TimestampsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) { }

        public TimestampsDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext) { }

        public TimestampsDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model) { }

        #endregion

        public override int SaveChanges()
        {
            UpdateEntriesTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            UpdateEntriesTimestamps();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateEntriesTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEntriesTimestamps()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is ITimestamps model)
                            model.CreatedAt = model.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDeletable deletedModel)
                        {
                            deletedModel.DeletedAt = DateTime.Now;
                            Entry(entry.Entity).State = EntityState.Modified;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is ITimestamps updatedModel)
                            updatedModel.UpdatedAt = DateTime.Now;
                        if (entry.Entity is ISoftDeletable forceDeletedModel && forceDeletedModel.DeletedAt == DateTime.MaxValue)
                            Entry(entry.Entity).State = EntityState.Deleted;
                        break;

                }
            }
        }
    }
}
