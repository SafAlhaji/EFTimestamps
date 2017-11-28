using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTimestamps.Extensions;
using EFTimestamps.Interfaces;

namespace EFTimestamps
{
    public class TimestampsContext : DbContext
    {
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry is ITimestamps model)
                            model.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        if (entry is ITimestamps updatedModel)
                            updatedModel.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        if (entry is ISoftDeletable deletedModel && deletedModel.DeletedAt != DateTime.MaxValue)
                        {
                            deletedModel.DeletedAt = DateTime.Now;
                            Entry(entry).State = EntityState.Modified;
                        }
                        break;
                }
            }

            return base.SaveChanges();
        }
    }
}
