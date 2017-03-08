using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// User DB transactions
    /// </summary>
    public class UserRepository
    {
        // Test data
        User[] users = new User[] 
        { 
            new User { Id = 1, Email = "test@exemple.com", Password = "mdp", AccessKey = "AKIAIOSFODNN7EXAMPLE" }, 
            new User { Id = 2, Email = "test2@exemple.com", Password = "password", AccessKey = "BITNHQ402LCLBPWTTLWZ" }
        };

        public User Get(Func<User, Boolean> where)
        {
            return users.Where(where).FirstOrDefault<User>();
        }
    }
}
