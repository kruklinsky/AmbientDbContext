using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Concrete.Mapping
{
    public static class UserMap
    {
        public static User ToBll(this DAL.Interface.User item)
        {
            return new User
            {
                Id = item.Id,
                Email = item.Email
            };
        }
    }
}
