using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;

namespace mpp_app_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ICollection<User> _users;

        public UserRepository(ICollection<User> users)
        {
            _users = users;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void DeleteUser(string id)
        {
            _users.Remove(GetUserById(id));
        }

        public User GetUserById(string id)
        {
            User? foundUser = _users.FirstOrDefault(user => user.ID == id);
            if (foundUser == null)
            {
                throw new UserNotFoundException();
            }
            return foundUser;
        }

        public ICollection<User> GetUsers()
        {
            return _users;
        }

        public void UpdateUser(string id, string username, string email, string password, string avatar,
            DateTime birthdate, DateTime registeredAt)
        {
            User userToUpdate = GetUserById(id);
            userToUpdate.Username = username;
            userToUpdate.Email = email;
            userToUpdate.Password = password;
            userToUpdate.Avatar = avatar;
            userToUpdate.Birthdate = birthdate;
            userToUpdate.RegisteredAt = registeredAt;
        }
    }
}
