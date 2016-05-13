using AmbientDbContext.cs;
using AmbientDbContext.Interface;
using BLL.Concrete;
using BLL.Interface.Abstract;
using DAL;
using DAL.Interface;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext.Infrastructure
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IAmbientDbContextLocator>().To<AmbientDbContextLocator>();
            Bind<IDbContextScopeFactory>().To<DbContextScopeFactory>().WithConstructorArgument((IDbContextFactory)null);
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserQueryService>().To<UserQueryService>();
        }
    }
}
