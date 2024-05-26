using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mpp_app_backend.Models
{
    public class User
    {
        [Key]
        public string ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableIndex { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public DateTime Birthdate { get; set; }

        public DateTime RegisteredAt { get; set; }

        public virtual ICollection<LoginActivity>? LoginActivities { get; set; }

        [ForeignKey("FK_User_AdminId")]
        public string AdminId { get; set; }
    }
}
