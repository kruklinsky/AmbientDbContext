using AmbientDbContext.Interface;
using BLL.Concrete;
using BLL.Interface.Abstract;
using DAL.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            Console.WriteLine("Write user id: (1 or 2)");
            BLL.Interface.Entities.User result = null;
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                IUserQueryService userQueryService = new UserQueryService(kernel.Get<IUserRepository>(), kernel.Get<IDbContextScopeFactory>());
                result = userQueryService.GetUser(input);
            }

            if (result != null)
            {
                Console.WriteLine(string.Format("Id: {0}", result.Id));
                Console.WriteLine(string.Format("Email: {0}", result.Email));
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("User not found.");
                Console.WriteLine("");
            }
        }
    }
}
