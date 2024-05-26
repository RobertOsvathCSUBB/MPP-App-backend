using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers(string adminID);
        ICollection<User> GetUsersPaginated(string adminID, int offset, int pageSize);
        User GetUserById(string id);
        void AddUser(User user);
        /*void AddUserRange(ICollection<User> users);*/
        void UpdateUser(User newUser);
        void DeleteUser(string id);
    }
}
