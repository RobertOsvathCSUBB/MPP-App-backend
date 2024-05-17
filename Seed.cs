using mpp_app_backend.Models;
using Bogus;
using mpp_app_backend.Context;
using Microsoft.EntityFrameworkCore;

namespace mpp_app_backend
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedDataContext()
        {
            var loginActivitiesFaker = new Faker<LoginActivity>();
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.ID, fake => fake.Random.Guid().ToString());
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.Time, fake => fake.Date.Past());
            loginActivitiesFaker.RuleFor(LoginActivity => LoginActivity.Latitude, fake => fake.Random.Double());
            loginActivitiesFaker.RuleFor(LoginActivity => LoginActivity.Longitude, fake => fake.Random.Double());
            loginActivitiesFaker.RuleFor(loginActivity => loginActivity.IP, fake => fake.Internet.Ip());

            var usersFaker = new Faker<User>();
            usersFaker.RuleFor(user => user.ID, fake => fake.Random.Uuid().ToString());
            usersFaker.RuleFor(user => user.Username, fake => fake.Internet.UserName());
            usersFaker.RuleFor(user => user.Email, fake => fake.Internet.Email());
            usersFaker.RuleFor(user => user.Password, fake => fake.Internet.Password());
            usersFaker.RuleFor(user => user.Avatar, fake => fake.Internet.Avatar());
            usersFaker.RuleFor(user => user.Birthdate, fake => fake.Date.Past());
            usersFaker.RuleFor(user => user.RegisteredAt, fake => fake.Date.Past());
            usersFaker.RuleFor(user => user.LoginActivities, (fake, user) => loginActivitiesFaker.Generate(3));

            if (!_context.Users.Any())
            {
                var users = usersFaker.Generate(10);
                _context.Database.SetCommandTimeout(300);
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
