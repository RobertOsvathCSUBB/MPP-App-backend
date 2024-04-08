using System.Collections.Generic;
using System.Security;
using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUserById(string id);
        void AddUser(User user);
        void UpdateUser(string id, string username, string email, string password, string avatar,
            DateTime birthdate, DateTime registeredAt);
        void DeleteUser(string id);
    }
}
