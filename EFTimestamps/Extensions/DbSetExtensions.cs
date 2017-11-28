using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTimestamps.Interfaces;

namespace EFTimestamps.Extensions
{
    public static class DbSetExtensions
    {
        public static T Restore<T>(this DbSet<T> dbSet, T obj) where T : class, ISoftDeletable
        {
            obj.DeletedAt = null;
            return obj;
        }

        public static IQueryable<T> Deleted<T>(this DbSet<T> dbSet) where T : class, ISoftDeletable
        {
            return dbSet.Where(c => c.DeletedAt != null);
        }

        public static IQueryable<T> Undeleted<T>(this DbSet<T> dbSet) where T : class, ISoftDeletable
        {
            return dbSet.Where(c => c.DeletedAt == null);
        }

        public static T ForceRemove<T>(this DbSet<T> dbSet, T obj) where T : class, ISoftDeletable
        {
            obj.DeletedAt = DateTime.MaxValue;
            return obj;
        }
    }
}
