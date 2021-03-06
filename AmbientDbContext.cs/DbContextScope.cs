﻿using AmbientDbContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmbientDbContext.cs
{
    /// <summary>
    /// Represents the entity of ambient DbContext.
    /// </summary>
    public class DbContextScope : IDbContextScope
    {
        private bool disposed;
        private bool readOnly;
        private bool completed;
        private bool nested;
        private DbContextScope parentScope;
        private DbContextCollection dbContexts;

        private void Initialize(bool readOnly)
        {
            this.disposed = false;
            this.completed = false;
            this.readOnly = readOnly;
            this.parentScope = GetAmbientScope();
        }

        public DbContextScope(bool readOnly, IDbContextFactory dbContextFactory = null)
        {
            this.Initialize(readOnly);

            if (this.parentScope == null)
            {
                this.Create(readOnly, dbContextFactory);
            }
            else
            {
                this.Nest();
            }

            SetAmbientScope(this);
        }

        #region IDbContextScope

        public IDbContextCollection DbContexts
        {
            get
            {
                return this.dbContexts;
            }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.DisposeDbContexts();
                this.ProtectDisposingOrder();
                RemoveAmbientScope();
                this.DisposeParent();
                this.disposed = true;
            }
        }

        #endregion

        #region Private methods

        private void Create(bool readOnly, IDbContextFactory dbContextFactory)
        {
            this.nested = false;
            this.dbContexts = new DbContextCollection(readOnly, dbContextFactory);
        }

        private void Nest()
        {
            if (this.parentScope.readOnly && !this.readOnly)
            {
                throw new InvalidOperationException("Cannot nest a read write DbContextScope within a read-only DbContextScope.");
            }
            this.nested = true;
            this.dbContexts = this.parentScope.dbContexts;
        }

        #region Dispose

        private void DisposeDbContexts()
        {
            if (!this.nested)
            {
                if (!this.completed)
                {
                    this.Complete();
                }
                this.dbContexts.Dispose();
            }
        }

        private void Complete()
        {
            this.completed = true;
        }

        private void ProtectDisposingOrder()
        {
            var currentAmbientScope = GetAmbientScope();
            if (currentAmbientScope != this)
            {
                throw new InvalidOperationException("DbContextScope instances must be disposed of in the order in which they were created.");
            }
        }

        private void DisposeParent()
        {
            if (this.parentScope != null)
            {
                if (this.parentScope.disposed)
                {
                    System.Diagnostics.Debug.WriteLine("DbContextScope parent is already disposed.");
                }
                else
                {
                    SetAmbientScope(this.parentScope);
                }
            }
        }

        #endregion

        #endregion

        #region Ambient Context Logic

        private static readonly string AmbientDbContextScopeKey = "AmbientDbcontext_" + Guid.NewGuid();
        private static readonly ConditionalWeakTable<InstanceIdentifier, DbContextScope> DbContextScopeInstances = new ConditionalWeakTable<InstanceIdentifier, DbContextScope>();
        private InstanceIdentifier _instanceIdentifier = new InstanceIdentifier();

        internal static void SetAmbientScope(DbContextScope newAmbientScope)
        {
            if (newAmbientScope == null)
            {
                throw new ArgumentNullException("newAmbientScope");
            }
            var current = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;
            if (current != newAmbientScope._instanceIdentifier)
            {
                CallContext.LogicalSetData(AmbientDbContextScopeKey, newAmbientScope._instanceIdentifier);
                DbContextScopeInstances.GetValue(newAmbientScope._instanceIdentifier, key => newAmbientScope);
            }
        }

        internal static void RemoveAmbientScope()
        {
            var current = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;
            CallContext.LogicalSetData(AmbientDbContextScopeKey, null);
            if (current != null)
            {
                DbContextScopeInstances.Remove(current);
            }
        }


        internal static DbContextScope GetAmbientScope()
        {
            var instanceIdentifier = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;
            if (instanceIdentifier != null)
            {
                DbContextScope ambientScope;
                if (DbContextScopeInstances.TryGetValue(instanceIdentifier, out ambientScope))
                {
                    return ambientScope;
                }
                System.Diagnostics.Debug.WriteLine("Programming error detected. Found a reference to an ambient DbContextScope in the CallContext but didn't have an instance for it in our DbContextScopeInstances table. This most likely means that this DbContextScope instance wasn't disposed of properly. DbContextScope instance must always be disposed. Review the code for any DbContextScope instance used outside of a 'using' block and fix it so that all DbContextScope instances are disposed of.");
            }
            return null;
        }

        #endregion
    }

    #region Ambient Context Logic

    #region This implementation is inspired by the source code of the TransactionScope class in .NET 4.5.1

    internal class InstanceIdentifier : MarshalByRefObject { }

    #endregion

    #endregion
}
