using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface IUserService
    {
        public ICollection<User> GetUsers();
        public ICollection<User> GetUsersSorted();
        public int GetTotalUsersCount();
        public ICollection<User> GetUsersPaginated(int page, int pageSize);
        public void AddUser(User user);
        public void UpdateUser(string id, User user);
        public void DeleteUser(string id);
        public User GetUserById(string id);
        public ICollection<IDictionary<string, int>> GetNumberOfUsersByRegistrationYear();
        public void AddLoginActivity(string userId, LoginActivity loginActivity);
        public void AddUserRange(ICollection<User> users);
    }
}
