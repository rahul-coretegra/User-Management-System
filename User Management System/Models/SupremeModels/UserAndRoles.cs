using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Models.SupremeModels
{
    public class UserAndRoles
    {
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [JsonIgnore]
        public string userAndRoleUniqueId { get; set; }

        [Required]
        public string userUniqueCode { get; set; }

        [JsonIgnore]
        public User user { get; set; }

        [Required]
        public string roleUniqueCode { get; set; }
        public UserRole userRole { get; set; }

        public TrueFalse accessToRole { get; set; }

    }
}
