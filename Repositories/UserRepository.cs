using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;
using mpp_app_backend.Context;
using System.Linq;

namespace mpp_app_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void AddUserRange(ICollection<User> users)
        {
            _context.AddRange(users);
            _context.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var user = GetUserById(id);
            _context.Remove(user);
            _context.SaveChanges();
        }

        public User GetUserById(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public ICollection<User> GetUsersPaginated(int offset, int pageSize)
        {
            int lastFetchedIndex = offset * pageSize;
            ICollection<User> users = _context.Users
                .OrderBy(user => user.TableIndex)
                .Where(user => user.TableIndex > lastFetchedIndex)
                .Take(pageSize)
                .ToList();
            return users;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUser(User user)
        {
            
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
