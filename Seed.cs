using Microsoft.AspNetCore.Builder;
using mpp_app_backend.Models;
using Bogus;
using mpp_app_backend.Interfaces;

namespace mpp_app_backend
{
    public class Seed
    {
        public static void SeedUsers(IServiceCollection services)
        {
            var usersFaker = new Faker<User>();
            usersFaker.RuleFor(user => user.ID, fake => fake.Random.Uuid().ToString());
            usersFaker.RuleFor(user => user.Username, fake => fake.Internet.UserName());
            usersFaker.RuleFor(user => user.Email, fake => fake.Internet.Email());
            usersFaker.RuleFor(user => user.Password, fake => fake.Internet.Password());
            usersFaker.RuleFor(user => user.Avatar, fake => fake.Internet.Avatar());
            usersFaker.RuleFor(user => user.Birthdate, fake => fake.Date.Past());
            usersFaker.RuleFor(user => user.RegisteredAt, fake => fake.Date.Past());

            var users = usersFaker.Generate(10);
            var repository = services.BuildServiceProvider().GetService<IUserRepository>();
            foreach (var user in users)
            {
                repository.AddUser(user);
            }
        }
    }
}
