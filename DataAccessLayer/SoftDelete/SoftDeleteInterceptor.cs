using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.SoftDelete
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public SoftDeleteInterceptor()
        {
           
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null)
                return result;

            try
            {
                foreach (var entry in eventData.Context.ChangeTracker.Entries())
                {
                    var IsDeletedproperty = entry.Entity.GetType().GetProperty("IsDeleted");
                    var dateOfDeletedProperty = entry.Entity.GetType().GetProperty("DateOfDeleted");

                    if (entry.State == EntityState.Deleted && IsDeletedproperty != null && IsDeletedproperty.PropertyType == typeof(bool)&&
                        dateOfDeletedProperty != null && IsDeletedproperty.PropertyType == typeof(DateTime?))
                    {
                        entry.State = EntityState.Modified;
                        IsDeletedproperty.SetValue(entry.Entity, true);
                        dateOfDeletedProperty.SetValue(entry.Entity, DateTime.UtcNow);
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot soft deleted");
            }

           
        }
    }
}
