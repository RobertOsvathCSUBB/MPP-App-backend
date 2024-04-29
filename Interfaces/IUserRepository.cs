using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        ICollection<User> GetUsersPaginated(int page, int pageSize);
        ICollection<User> GetUsersSorted();
        User GetUserById(string id);
        void AddUser(User user);
        void UpdateUser(User newUser);
        void DeleteUser(string id);
    }
}
