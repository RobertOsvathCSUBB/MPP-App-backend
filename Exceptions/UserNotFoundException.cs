using System;

namespace mpp_app_backend.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found")
        {
        }
    }
}
