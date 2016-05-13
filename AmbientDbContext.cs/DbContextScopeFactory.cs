using AmbientDbContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext.cs
{
    public class DbContextScopeFactory : IDbContextScopeFactory
    {
        private readonly IDbContextFactory dbContextFactory;

        public DbContextScopeFactory(IDbContextFactory dbContextFactory = null)
        {
            this.dbContextFactory = dbContextFactory;
        }

        #region IDbContextScopeFactory

        public IDbContextReadOnlyScope CreateReadOnly()
        {
            return new DbContextReadOnlyScope(this.dbContextFactory);
        }

        #endregion
    }
}
