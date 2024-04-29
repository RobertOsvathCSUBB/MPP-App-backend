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
        private readonly Faker<User> usersFaker;
        private readonly Faker<LoginActivity> loginActivitiesFaker;
        private TimeSpan newUserInterval = TimeSpan.FromSeconds(1);

        public DataRefreshHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            loginActivitiesFaker = new Faker<LoginActivity>();
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.ID, fake => fake.Random.Uuid().ToString());
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.Time, fake => fake.Date.Past());
            loginActivitiesFaker.RuleFor(LoginActivity => LoginActivity.Latitude, fake => fake.Random.Double());
            loginActivitiesFaker.RuleFor(LoginActivity => LoginActivity.Longitude, fake => fake.Random.Double());
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.IP, fake => fake.Internet.Ip());

            usersFaker = new Faker<User>();
            usersFaker.RuleFor(user => user.ID, fake => fake.Random.Uuid().ToString());
            usersFaker.RuleFor(user => user.Username, fake => fake.Internet.UserName());
            usersFaker.RuleFor(user => user.Email, fake => fake.Internet.Email());
            usersFaker.RuleFor(user => user.Password, fake => fake.Internet.Password());
            usersFaker.RuleFor(user => user.Avatar, fake => fake.Internet.Avatar());
            usersFaker.RuleFor(user => user.Birthdate, fake => fake.Date.Past());
            usersFaker.RuleFor(user => user.RegisteredAt, fake => fake.Date.Past());
            usersFaker.RuleFor(user => user.LoginActivities, loginActivitiesFaker.Generate(10000));
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
                var newUser = usersFaker.Generate();
                _userRepository.AddUser(newUser);
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                string newUserJson = JsonConvert.SerializeObject(newUser, serializerSettings);
                await Clients.All.SendAsync("ReceiveNewUser", newUserJson);
            }
        }
    }
}
