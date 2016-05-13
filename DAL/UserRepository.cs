using AmbientDbContext.Interface;
using DAL.Interface;
using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        #region IRepository

        private readonly IAmbientDbContextLocator ambientDbContextLocator;

        private DbContext context
        {
            get
            {
                var dbContext = this.ambientDbContextLocator.Get<DbContext>();
                if (dbContext == null)
                {
                    throw new InvalidOperationException("It is impossible to use repository because DbContextScope has not been created.");
                }
                return dbContext;
            }
        }

        public UserRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null)
            {
                throw new System.ArgumentNullException("ambientDbContextLocator", "Ambient dbContext locator is null.");
            }
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        #endregion


        public User GetUser(string id)
        {
            User result = null;
            ORM.Model.User user = null;

            var query = this.context.Users.Where(u => u.UserId == id);
            if (query.Count() != 0)
            {
                user = query.First();
            }

            if (user != null)
            {
                result = new User
                {
                    Id = user.UserId,
                    Email = user.Email
                };
            }


            return result;
        }
    }
}
