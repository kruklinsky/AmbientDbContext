using AmbientDbContext.Interface;
using BLL.Interface.Abstract;
using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Concrete.Mapping;

namespace BLL.Concrete
{
    public class UserQueryService: RepositoryService<DAL.Interface.IUserRepository>, IUserQueryService
    {
        public UserQueryService(DAL.Interface.IUserRepository userRepository, IDbContextScopeFactory dbContextScopeFactory) : base(userRepository, dbContextScopeFactory) { }

         public User GetUser(string id) {
             User result = null;

             using (var context = dbContextScopeFactory.CreateReadOnly())
             {
                 var user = this.repository.GetUser(id);
                 if (user != null)
                 {
                     result = user.ToBll();
                 }
             }

             return result;
         }
    }
}
