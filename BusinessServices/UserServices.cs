using DataModel;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    /// <summary>
    /// User services
    /// </summary>
    public class UserServices : IUserServices
    {
        private readonly UserRepository _userRepository;

        public UserServices()
        {
            this._userRepository = new UserRepository();
        }

        /// <summary>
        /// Authenticate user by email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>true if user exists, false otherwise</returns>
        public bool Authenticate(string email, string password)
        {
            User user = _userRepository.Get(u => (u.Email == email && u.Password == password));

            return user != null ? true : false;
        }

        /// <summary>
        /// Get a user secret access key.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetAccessKey(string email)
        {
            User user = _userRepository.Get(u => u.Email == email);

            return user != null ? user.AccessKey : null;
        }
    }
}
