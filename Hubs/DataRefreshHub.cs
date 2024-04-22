using Microsoft.AspNetCore.SignalR;
using mpp_app_backend.Interfaces;
using Bogus;
using mpp_app_backend.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Serialization;

namespace mpp_app_backend.Hubs
{
    [EnableCors("AllowFrontendOrigin")]
    public class DataRefreshHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly Faker<User> _userFaker;
        private TimeSpan newUserInterval = TimeSpan.FromSeconds(10);

        public DataRefreshHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userFaker = new Faker<User>();
            _userFaker.RuleFor(user => user.ID, fake => fake.Random.Uuid().ToString());
            _userFaker.RuleFor(user => user.Username, fake => fake.Internet.UserName());
            _userFaker.RuleFor(user => user.Email, fake => fake.Internet.Email());
            _userFaker.RuleFor(user => user.Password, fake => fake.Internet.Password());
            _userFaker.RuleFor(user => user.Avatar, fake => fake.Internet.Avatar());
            _userFaker.RuleFor(user => user.Birthdate, fake => fake.Date.Past());
            _userFaker.RuleFor(user => user.RegisteredAt, fake => fake.Date.Past());
        }

        public override async Task OnConnectedAsync()
        {
            await CreateAndSendNewUser();
        }

        public async Task CreateAndSendNewUser()
        {
            while (true)
            {
                await Task.Delay(newUserInterval);
                var newUser = _userFaker.Generate();
                _userRepository.AddUser(newUser);
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                string newUserJson = JsonConvert.SerializeObject(newUser, serializerSettings);
                await Clients.All.SendAsync("ReceiveNewUser", newUserJson);
            }
        }
    }
}
