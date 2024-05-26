using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface IUserService
    {
        public ICollection<User> GetUsers(string adminID);
        /*public ICollection<User> GetUsersSorted();*/
        public int GetTotalUsersCount(string adminID);
        public ICollection<User> GetUsersPaginated(string adminID, int page, int pageSize);
        public void AddUser(string adminID, User user);
        public void UpdateUser(string id, User user);
        public void DeleteUser(string id);
        public User GetUserById(string id);
        /*public ICollection<IDictionary<string, int>> GetNumberOfUsersByRegistrationYear();*/
        public void AddLoginActivity(string userId, LoginActivity loginActivity);
        /*public void AddUserRange(string adminID, ICollection<User> users);*/
    }
}
