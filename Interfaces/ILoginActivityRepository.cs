using mpp_app_backend.Models;

namespace mpp_app_backend.Interfaces
{
    public interface ILoginActivityRepository
    {
        /*ICollection<LoginActivity> GetLoginActivities();*/
        LoginActivity GetLoginActivityById(string id);
        ICollection<LoginActivity> GetLoginActivitiesByUserId(string id);
        void AddLoginActivity(LoginActivity loginActivity);
        /*void UpdateLoginActivity(LoginActivity newLoginActivity);
        void DeleteLoginActivity(string id);*/
    }
}
