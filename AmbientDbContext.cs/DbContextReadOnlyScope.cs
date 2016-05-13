using AmbientDbContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext.cs
{
    public class DbContextReadOnlyScope : IDbContextReadOnlyScope
    {
        private DbContextScope internalScope;

        public DbContextReadOnlyScope(IDbContextFactory dbContextFactory = null)
        {
            this.internalScope = new DbContextScope(true, dbContextFactory);
        }

        #region IDbContextReadOnlyScope

        public IDbContextCollection DbContexts
        {
            get
            {
                return this.internalScope.DbContexts;
            }
        }
        public void Dispose()
        {
            this.internalScope.Dispose();
        }

        #endregion
    }
}
