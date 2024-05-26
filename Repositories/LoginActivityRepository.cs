using mpp_app_backend.Models;
using mpp_app_backend.Context;
using mpp_app_backend.Exceptions;
using mpp_app_backend.Interfaces;

namespace mpp_app_backend.Repositories
{
    public class LoginActivityRepository : ILoginActivityRepository
    {
        private readonly DataContext _context;

        public LoginActivityRepository(DataContext context)
        {
            _context = context;
        }

        public void AddLoginActivity(LoginActivity loginActivity)
        {
            _context.Add(loginActivity);
            _context.SaveChanges();
        }

        /*public void DeleteLoginActivity(string id)
        {
            var loginActivity = GetLoginActivityById(id);
            _context.Remove(loginActivity);
            _context.SaveChanges();
        }*/

        public LoginActivity GetLoginActivityById(string id)
        {
            var loginActivity = _context.LoginActivities.Find(id);
            if (loginActivity == null)
            {
                throw new LoginActivityNotFoundException();
            }
            return loginActivity;
        }

        public ICollection<LoginActivity> GetLoginActivities()
        {
            return _context.LoginActivities.ToList();
        }

        /*public void UpdateLoginActivity(LoginActivity loginActivity)
        {
            _context.Update(loginActivity);
            _context.SaveChanges();
        }*/

        public ICollection<LoginActivity> GetLoginActivitiesByUserId(string id)
        {
            var loginActivities = _context.LoginActivities.Where(loginActivity => loginActivity.UserId == id).ToList();
            if (loginActivities == null)
            {
                throw new LoginActivityNotFoundException();
            }
            return loginActivities;
        }
    }
}
