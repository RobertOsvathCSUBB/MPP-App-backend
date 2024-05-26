using mpp_app_backend.Interfaces;
using mpp_app_backend.Models;
using System.Collections;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace mpp_app_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginActivityRepository _loginActivityRepository;

        public UserService(IUserRepository userRepository, ILoginActivityRepository loginActivityRepository)
        {
            _userRepository = userRepository;
            _loginActivityRepository = loginActivityRepository;
        }

        public ICollection<User> GetUsers(string adminID)
        {
            return _userRepository.GetUsers(adminID);
        }

        public User GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }

        public int GetTotalUsersCount(string adminID)
        {
            return _userRepository.GetUsers(adminID).Count;
        }

        public ICollection<User> GetUsersPaginated(string adminID, int page, int pageSize)
        {
            return _userRepository.GetUsersPaginated(adminID, page, pageSize);
        }

        /*public ICollection<User> GetUsersSorted()
        {
            return _userRepository.GetUsers().OrderBy(user => user.Username).ToList();
        }*/

        public void AddUser(string adminID, User user)
        {
            while (true)
            {
                try
                {
                    user.ID = Guid.NewGuid().ToString();
                    user.AdminId = adminID;
                    _userRepository.AddUser(user);
                    break;
                }
                catch (DbUpdateException)
                {
                    continue;
                }
            }
        }

        /*public void AddUserRange(ICollection<User> users)
        {
            foreach (User user in users)
            {
                user.ID = Guid.NewGuid().ToString();
            }
            _userRepository.AddUserRange(users);
        }*/

        public void UpdateUser(string id, User user)
        {
            user.ID = id;
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(string id)
        {
            _userRepository.DeleteUser(id);
        }

        /*public ICollection<IDictionary<string, int>> GetNumberOfUsersByRegistrationYear()
        {
            ICollection<User> users = _userRepository.GetUsers();
            IDictionary<int, int> nrOfUsersByUniqueYears = new Dictionary<int, int>();
            foreach (User user in users)
            {
                if (nrOfUsersByUniqueYears.ContainsKey(user.RegisteredAt.Year))
                {
                    nrOfUsersByUniqueYears[user.RegisteredAt.Year]++;
                }
                else
                {
                    nrOfUsersByUniqueYears.Add(user.RegisteredAt.Year, 1);
                }
            }
            ICollection<IDictionary<string, int>> result = new List<IDictionary<string, int>>();
            foreach (KeyValuePair<int, int> entry in nrOfUsersByUniqueYears)
            {
                IDictionary<string, int> dict = new Dictionary<string, int>();
                dict.Add("year", entry.Key);
                dict.Add("users", entry.Value);
                result.Add(dict);
            }
            return result;
        }*/

        public void AddLoginActivity(string userId, LoginActivity loginActivity)
        {
            while (true)
            {
                try
                {
                    loginActivity.ID = Guid.NewGuid().ToString();
                    loginActivity.UserId = userId;
                    _loginActivityRepository.AddLoginActivity(loginActivity);
                    break;
                }
                catch (DbUpdateException)
                {
                    continue;
                }
            }
        }
    }
}
