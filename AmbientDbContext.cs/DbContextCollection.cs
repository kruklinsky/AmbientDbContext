using AmbientDbContext.Interface;
using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmbientDbContext.cs
{
    /// <summary>
    /// Contains and manages dbContext instances.
    /// </summary>
    public class DbContextCollection : IDbContextCollection
    {
        private bool disposed;
        private bool completed;
        private bool readOnly;
        private Dictionary<Type, DbContext> initializedDbContexts;
        private readonly IDbContextFactory dbContextFactory;

        internal Dictionary<Type, DbContext> InitializedDbContexts
        {
            get
            {
                return this.initializedDbContexts;
            }
        }

        private void Initialize()
        {
            this.disposed = false;
            this.completed = false;
            this.initializedDbContexts = new Dictionary<Type, DbContext>();
        }

        public DbContextCollection(bool readOnly = false, IDbContextFactory dbContextFactory = null)
        {
            this.Initialize();
            this.readOnly = readOnly;
            this.dbContextFactory = dbContextFactory;
        }

        #region Public methods

        public TDbContext Get<TDbContext>() where TDbContext : DbContext
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("DbContextCollection", "DbContextCollection is disposed.");
            }
            var requestedType = typeof(TDbContext);
            if (!this.initializedDbContexts.ContainsKey(requestedType))
            {
                this.AddDbContext<TDbContext>(requestedType);
            }
            return this.initializedDbContexts[requestedType] as TDbContext;
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (!this.completed) Complete();
                foreach (var dbContext in this.initializedDbContexts.Values)
                {
                    this.DisposeDbContext(dbContext);
                }
                this.initializedDbContexts.Clear();
                this.disposed = true;
            }
        }

        #endregion

        #region Private methods

        private void Complete()
        {
            this.completed = true;
        }

        private void AddDbContext<TDbContext>(Type requestedType) where TDbContext : DbContext
        {
            var dbContext = this.dbContextFactory != null
                ? this.dbContextFactory.CreateDbContext<TDbContext>()
                : Activator.CreateInstance<TDbContext>();
            this.initializedDbContexts.Add(requestedType, dbContext);
            if (this.readOnly)
            {
                dbContext.ReadOnly = true;
            }
        }

        private void DisposeDbContext(DbContext dbContext)
        {
            try
            {
                dbContext.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion
    }
}
