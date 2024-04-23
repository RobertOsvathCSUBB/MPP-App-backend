using System;

namespace mpp_app_backend.Exceptions
{
    public class LoginActivityNotFoundException : Exception
    {
        public LoginActivityNotFoundException() : base("LoginActivity not found")
        {
        }
    }
}
