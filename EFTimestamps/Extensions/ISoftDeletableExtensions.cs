using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTimestamps.Interfaces;

namespace EFTimestamps.Extensions
{
    public static class ISoftDeletableExtensions
    {
        public static bool IsDeleted(this ISoftDeletable model)
        {
            return model.DeletedAt != null;
        }
    }
}
