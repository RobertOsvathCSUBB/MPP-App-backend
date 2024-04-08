namespace mpp_app_backend.Models
{
    public class User
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
