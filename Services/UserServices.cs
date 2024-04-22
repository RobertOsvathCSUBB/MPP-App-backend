using mpp_app_backend.Interfaces;
using mpp_app_backend.Models;
using System.Collections;
using System.Collections.Generic;

namespace mpp_app_backend.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ICollection<IDictionary<string, int>> GetNumberOfUsersByRegistrationYear()
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
        }
    }
}
