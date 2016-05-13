using AmbientDbContext.Interface;
using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext.cs
{
    public class AmbientDbContextLocator : IAmbientDbContextLocator
    {
        public TDbContext Get<TDbContext>() where TDbContext : DbContext
        {
            var ambientDbContextScope = DbContextScope.GetAmbientScope();
            TDbContext result = ambientDbContextScope == null
                ? null
                : ambientDbContextScope.DbContexts.Get<TDbContext>();
            return result;
        }
    }
}
