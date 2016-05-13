using ORM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ORM
{
    public class DbContext : IDisposable
    {
        public bool ReadOnly {get; set;}

        public void Dispose()
        {
            Console.WriteLine("");
            Console.WriteLine("Context Diposed.");
            Console.WriteLine("");
        }

        public List<User> Users
        {
            get
            {
                return new List<User>{

                    new User {
                        UserId = "1",
                        Email = "kin-1983@tut.by"
                    }, new User {
                        UserId = "2",
                        Email = "kruklinsky.alexander@gmail.com"
                    }
                };
            }
        }

    }
}
