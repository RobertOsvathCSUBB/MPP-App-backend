namespace mpp_app_backend.Models
{
    public class LoginActivity
    {
        public string ID { get; set; }
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string IP { get; set; }
        public string UserId { get; set; }
    }
}
